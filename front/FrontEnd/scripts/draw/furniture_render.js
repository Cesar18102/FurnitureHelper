let OBJLoader = new THREE.OBJLoader();

let SCENE_RENDER = undefined;
let SCENE_RENDER_PROMISE = import("./scene_render.js").then(module => SCENE_RENDER = module);

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
		OBJLoader.load(furnitureInfo.model_url, model => {
			for(let used of furnitureInfo.used_parts) {
				let partObject = model.getObjectByName(used.id.toString());
				let partInfo = getPart(furnitureInfo, part => part.id == used.part_id);
				
				let texture = new THREE.TextureLoader().load(partInfo.possible_materials[0].texture_url);
				let material = new THREE.MeshPhysicalMaterial({ 
					map : texture, 
					flatShading : true
				});
				
				let mesh = new THREE.Mesh(partObject.geometry, material);
				
				sceneInfo.scene.add(mesh);
				partMeshes[used.id] = mesh;
				
				if(prepare != undefined) {
					prepare(mesh);
				}
				
				render(sceneInfo.renderer, sceneInfo.scene, sceneInfo.camera, motion == undefined ? undefined : () => motion(mesh));
				
				resolve({
					sceneInfo : sceneInfo,
					partMeshesInfo : partMeshes
				});
			}
			
		}, undefined, error => {
			reject(error);
		})
	});
}

function render(renderer, scene, camera, motion) {
	requestAnimationFrame(() => render(renderer, scene, camera, motion));
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