let OBJLoader = new THREE.OBJLoader();

let SCENE_RENDER = undefined;
let SCENE_RENDER_PROMISE = import("/FurnitureFrontEnd/scripts/draw/scene_render.js").then(module => SCENE_RENDER = module);

let SHADERS = undefined;
let SHADERS_PROMISE = import("/FurnitureFrontEnd/scripts/draw/shaders.js").then(module => SHADERS = module);

async function renderFurniture(furnitureInfo, domWrapper, style, prepare, motion) {			
	if(SCENE_RENDER == undefined) {
		await SCENE_RENDER_PROMISE;
	}
	
	let sceneInfo = SCENE_RENDER.renderScene(domWrapper, style);
	let partMeshes = {};
	
	if(furnitureInfo.model_url == "") {
		return null;
	}
	
	return new Promise((resolve, reject) => {
		OBJLoader.load(
			furnitureInfo.model_url, 
			model => {
				for(let used of furnitureInfo.used_parts) {
					let partObject = model.getObjectByName(used.id.toString() + "U");
						
					if(partObject == null || partObject == undefined) { //useless due to the cycle throw used_parts
						partObject = model.getObjectByName(used.part_id.toString() + "M");
					}
						
					let partInfo = getPart(furnitureInfo, part => part.id == used.part_id);
					let texture = new THREE.TextureLoader().load(partInfo.possible_materials[0].texture_url);
					
					let material = new THREE.MeshPhysicalMaterial({ 
						map : texture, 
						flatShading : true
					});
								
					material.userData = {
						outline : { value : false },
						outlineColor : { value : new THREE.Color(0) },
						timing : { value : false },
						time : { value : (Date.now() % 85536000) / 1000 }
						//outlineStrength : { value : 10.0 }
					}
								
					material.onBeforeCompile = shader => {
						shader.uniforms.outline = material.userData.outline;
						shader.uniforms.outlineColor = material.userData.outlineColor;
						shader.uniforms.outlineStrength = material.userData.outlineStrength;
						shader.uniforms.time = material.userData.time;
						shader.uniforms.timing = material.userData.timing;
								
						shader.uniforms.sampler = { value : texture };
						//shader.uniforms.textureSize = { value : new THREE.Vector2(texture.image.width, texture.image.height) };
						shader.uniforms.lightPos = { value : sceneInfo.light.position };
							
						shader.vertexShader = SHADERS.defaultVertex();
						shader.fragmentShader = SHADERS.defaultFragment();
					};
								
					let mesh = new THREE.Mesh(partObject.geometry, material);
								
					mesh.info = partInfo;
					mesh.hasOutline = false;
					mesh.used_id = used.id;
					
					mesh.toggleOutline = togglePartOutline.bind(mesh);
					mesh.toggleTiming = toggleTiming.bind(mesh);
								
					sceneInfo.scene.add(mesh);
					partMeshes[used.id] = mesh;
								
					if(prepare != undefined) {
						prepare(mesh);
					}
								
					render(
						sceneInfo.renderer, 
						sceneInfo.scene, 
						sceneInfo.camera, 
						motion == undefined ? undefined : () => {
							mesh.material.userData.time.value = (Date.now() % 85536000) / 1000;
							motion(mesh);
						}
					);
					
					let interaction = new THREE.Interaction(sceneInfo.renderer, sceneInfo.scene, sceneInfo.camera);
								
					resolve({
						sceneInfo : sceneInfo,
						interaction : interaction,
						partMeshesInfo : partMeshes
					});
				}
			}, undefined, error => {
				reject(error);
			}
		)
	});
}

function toggleTiming() {
	this.material.userData.timing.value = !this.material.userData.timing.value;
}

function togglePartOutline(color) {
	this.hasOutline = !this.hasOutline;
	this.material.userData.outline.value = this.hasOutline;
	
	if(color != undefined && color != null) {
		this.material.userData.outlineColor.value = color;
	}
}

function render(renderer, scene, camera, motion) {
	requestAnimationFrame(
		() => render(renderer, scene, camera, motion)
	);
	renderer.render(scene, camera);
	
	if(motion != undefined) {
		motion();
	}
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