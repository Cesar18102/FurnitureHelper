let PART_RENDER = undefined;
let PART_RENDER_PROMISE = import("./part_render.js").then(module => PART_RENDER = module);
			
let SCENE_RENDER = undefined;
let SCENE_RENDER_PROMISE = import("./scene_render.js").then(module => SCENE_RENDER = module);

const UX = new THREE.Vector3(1, 0, 0);
const UY = new THREE.Vector3(0, 1, 0);
const UZ = new THREE.Vector3(0, 0, 1);

async function renderFurniture(furnitureInfo, domWrapper, style) {
	if(PART_RENDER == undefined) {
		await PART_RENDER_PROMISE;
	}
				
	if(SCENE_RENDER == undefined) {
		await SCENE_RENDER_PROMISE;
	}
	
	let sceneInfo = SCENE_RENDER.renderScene(domWrapper, style);
	
	let usedInfo = {};
	for(let used of furnitureInfo.used_parts) {
		let part = getPart(furnitureInfo, p => p.id == used.id);
		if(part == null) {
			part = getPart(furnitureInfo, p => p.id == used.part_id);
		}
		
		let mesh = await PART_RENDER.renderPart(getPartRenderInfo(part), sceneInfo, 
			p => { 
				scalePart(furnitureInfo, part, p); 
			},
			p => {
				//p.rotation.y += 0.01;
				//p.rotateZ(0.01);
			}
		);
		
		//X - Y
		//Y - X
		
		mesh.geometry.computeBoundingBox();   
		
		usedInfo[used.id] = { 
			partInfo : part,
			partMesh : mesh
		};
	}
	
	for(let globalConnection of furnitureInfo.global_connections) {				
		for(let connection of globalConnection.sub_connections) {
			let part = usedInfo[connection.used_part_id];
			let partOther = usedInfo[connection.used_part_other_id];
			
			let helperInfo = getHelperInfo(part.partMesh, connection.connection_helper);
			let helperInfoOther = getHelperInfo(partOther.partMesh, connection.connection_helper_other);
			
			helperInfo.helperMeshes = mergeWithHelpers(part.partMesh, helperInfo);
			helperInfoOther.helperMeshes = mergeWithHelpers(partOther.partMesh, helperInfoOther);
			
			let triangleInfo = getTriangleInfo(helperInfo, false);
			let triangleInfoOther = getTriangleInfo(helperInfoOther, true);
			
			let sub = new THREE.Vector3().subVectors(triangleInfo.normal, triangleInfoOther.normal).normalize();
			
			/*transformMesh(
				helperInfoOther, 
				mesh => {
					mesh.position.x += 50;
					mesh.rotateY(3.14 / 2);
				}
			);*/
			
			
			transformMesh(
				helperInfoOther, 
				helperInfo => {
					let ax = Math.atan2(Math.sqrt(sub.y * sub.y + sub.z * sub.z), sub.x);
					let ay = Math.atan2(Math.sqrt(sub.z * sub.z + sub.x * sub.x), sub.y);
					let az = Math.atan2(Math.sqrt(sub.x * sub.x + sub.y * sub.y), sub.z);
					
					helperInfo.mesh.rotateX(ax).rotateY(ay).rotateZ(az);
					helperInfo.positions.helperPosition.applyAxisAngle(UX, ax).applyAxisAngle(UY, ay).applyAxisAngle(UZ, az);
					helperInfo.positions.helperPositionOther.applyAxisAngle(UX, ax).applyAxisAngle(UY, ay).applyAxisAngle(UZ, az);
					helperInfo.positions.helperPositionHelp.applyAxisAngle(UX, ax).applyAxisAngle(UY, ay).applyAxisAngle(UZ, az);
				},
				helperMesh => {
					helperMesh.rotateX(Math.atan2(Math.sqrt(sub.y * sub.y + sub.z * sub.z), sub.x));
					helperMesh.rotateY(Math.atan2(Math.sqrt(sub.z * sub.z + sub.x * sub.x), sub.y));
					helperMesh.rotateZ(Math.atan2(Math.sqrt(sub.x * sub.x + sub.y * sub.y), sub.z));
				}
			);
			
			let triangleInfoUpdated = getTriangleInfo(helperInfo, false);
			let triangleInfoOtherUpdated = getTriangleInfo(helperInfoOther, false);
			
			let delta = getDelta(helperInfo, helperInfoOther);
			
			transformMesh(
				helperInfoOther,
				helperInfo => {
					helperInfo.mesh.position.add(delta)
					helperInfo.positions.helperPosition.add(delta);
					helperInfo.positions.helperPositionOther.add(delta);
					helperInfo.positions.helperPositionHelp.add(delta);
				},
				helperMesh => {
					helperMesh.position.add(delta);
				}
			);
			
			drawHelperPositions(helperInfo, sceneInfo.scene);
			drawHelperPositions(helperInfoOther, sceneInfo.scene);
			
			sceneInfo.scene.add(new THREE.Box3Helper(part.partMesh.geometry.boundingBox, 0xff0000));
			sceneInfo.scene.add(new THREE.Box3Helper(partOther.partMesh.geometry.boundingBox, 0xff0000));
			
			for(let glue of connection.connection_glues) {
				
			}
		}
		
		for(let glue of globalConnection.global_connections_glues) {
			
		}
	}
}

function getDelta(helperInfo, helperInfoOther) {	
	let delta = new THREE.Vector3().subVectors(helperInfo.positions.helperPosition, helperInfoOther.positions.helperPosition);
	let deltaOther = new THREE.Vector3().subVectors(helperInfo.positions.helperPositionOther, helperInfoOther.positions.helperPositionOther);
	let deltaHelp = new THREE.Vector3().subVectors(helperInfo.positions.helperPositionHelp, helperInfoOther.positions.helperPositionHelp);
	
	return new THREE.Vector3().add(delta).add(deltaOther).add(deltaHelp).divideScalar(3.0);
}

function transformMesh(helperInfo, transformFunction, helperTransformFunction) {
	helperInfo.mesh.geometry.boundingBox = null;
	
	transformFunction(helperInfo);
	
	helperTransformFunction(helperInfo.helperMeshes.helperMesh);
	helperTransformFunction(helperInfo.helperMeshes.helperMeshOther);
	helperTransformFunction(helperInfo.helperMeshes.helperMeshHelp);
	
	helperInfo.mesh.geometry.boundingBox = new THREE.Box3().expandByObject(helperInfo.mesh);
}

function getTriangleInfo(helperInfo, negateNormal) {
	let triangle = new THREE.Triangle(
		helperInfo.positions.helperPosition,//.applyMatrix4(helperInfo.helperMeshes.helperMesh.matrix),
		helperInfo.positions.helperPositionOther,//.applyMatrix4(helperInfo.helperMeshes.helperMeshOther.matrix),
		helperInfo.positions.helperPositionHelp,//.applyMatrix4(helperInfo.helperMeshes.helperMeshHelp.matrix),
	);
	
	let center = new THREE.Vector3();
	triangle.getMidpoint(center);
	//center.applyMatrix4(helperInfo.mesh.matrix);
	
	let normal = new THREE.Vector3();
	triangle.getNormal(normal);
	//normal.applyMatrix4(helperInfo.mesh.matrix);
	
	if(negateNormal) {
		normal.negate();
	}
	
	return {
		triangle : triangle,
		center : center,
		normal : normal
	};
}

function drawHelperPositions(helperInfo, scene) {
	scene.add(helperInfo.helperMeshes.helperMesh);
	scene.add(helperInfo.helperMeshes.helperMeshOther);
	scene.add(helperInfo.helperMeshes.helperMeshHelp);
}

function mergeWithHelpers(mesh, helperInfo) {
	let helpersGeometry = new THREE.Geometry();
	return 	{
		helperMesh : mergeWithHelper(helperInfo.positions.helperPosition, helpersGeometry),
		helperMeshOther : mergeWithHelper(helperInfo.positions.helperPositionOther, helpersGeometry),
		helperMeshHelp : mergeWithHelper(helperInfo.positions.helperPositionHelp, helpersGeometry),
		geometry : helpersGeometry
	};
}

function getHelperMesh(helperPosition) {
	let geometry = new THREE.SphereGeometry(2, 32, 32);
	let material = new THREE.MeshBasicMaterial({ color: 0 });
	let sphere = new THREE.Mesh(geometry, material);
	sphere.geometry.translate(helperPosition.x, helperPosition.y, helperPosition.z);
	return sphere;
}

function mergeWithHelper(helperPosition, geometry) {
	let helper = getHelperMesh(helperPosition);
	helper.updateMatrix();
	geometry.merge(helper.geometry, helper.matrix);
	return helper;
}

function getHelperInfo(mesh, helper) {
	return {
		mesh : mesh,
		helper : helper,
		positions : getHelperPositions(mesh, helper)
	};
}

function getHelperPositions(partMesh, helper) {
	let min = partMesh.geometry.boundingBox.min;
	let size = partMesh.geometry.boundingBox.getSize();

	return {
		helperPosition : new THREE.Vector3(min.x + size.x * helper.pos_x, min.y + size.y * helper.pos_y, min.z + size.z * helper.pos_z),
		helperPositionOther : new THREE.Vector3(min.x + size.x * helper.pos_x_other, min.y + size.y * helper.pos_y_other, min.z + size.z * helper.pos_z_other),
		helperPositionHelp : new THREE.Vector3(min.x + size.x * helper.pos_x_help, min.y + size.y * helper.pos_y_help, min.z + size.z * helper.pos_z_help)
	};
}

function getPart(furnitureInfo, predicate) {
	for(let globalConnection of furnitureInfo.global_connections) {
		for(let glue of globalConnection.global_connections_glues) {
			if(predicate(glue.glue_part)) {
				return glue.glue_part;
			}
		}			
		for(let connection of globalConnection.sub_connections) {
			if(predicate(connection.part)) {
				return connection.part;
			}
			if(predicate(connection.part_other)) {
				return connection.part_other;
			}
			for(let glue of connection.connection_glues) {
				if(predicate(glue.glue_part)) {
					return glue.glue_part;
				}
			}
		}
	}		
	return null;
}

function scalePart(furnitureInfo, partInfo, partMesh) {
	let scale = furnitureInfo.scale * partInfo.in_furniture_scale;
	partMesh.scale(scale, scale, scale);
}

function getPartRenderInfo(part) {
	return {
		model_url : part.model_url,
		texture_url : part.possible_materials[0].texture_url
	};
}