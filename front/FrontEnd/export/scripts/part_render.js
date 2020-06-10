let OBJLoader = new THREE.OBJLoader();

/*let SHADERS = undefined;
let SHADERS_PROMISE = import("/FurnitureFrontEnd/scripts/draw/shaders.js").then(module => SHADERS = module);*/

async function renderPart(part, renderInfo, prepare, motion, cameraMotion) {
	/*if(SHADERS == undefined) {
		await SHADERS_PROMISE;
	}*/
	
	let red = undefined, green = undefined, blue = undefined, alpha = undefined;
	if(part.color != undefined) {
		red = parseInt(part.color.substr(0, 2), 16) / 256;
		green = parseInt(part.color.substr(2, 2), 16) / 256;
		blue = parseInt(part.color.substr(4, 2), 16) / 256;
		alpha = parseInt(part.color.substr(6, 2), 16) / 256;
	}
	
	return new Promise((resolve, reject) => {
		OBJLoader.load(part.model_url, object => {
			let texture = new THREE.TextureLoader().load(part.texture_url);
			
			let material = undefined;
			
			if(renderInfo.light != undefined) {
				material = new THREE.MeshPhysicalMaterial({ 
					map : texture, 
					flatShading : true
				});
				
				material.onBeforeCompile = shader => {
					if(part.color != undefined) {
						shader.uniforms.mycolor = { value : new THREE.Color(red * alpha, green * alpha, blue * alpha) };
					}
									
					shader.vertexShader = defaultVertex();
					shader.fragmentShader = defaultFragment(part.color != undefined);
				};
			} else {
				material = new THREE.MeshNormalMaterial({ 
					flatShading : true
				});
			}
							
			let geometry = object.children[0].geometry;
			let mesh = new THREE.Mesh(geometry, material);
			renderInfo.scene.add(mesh);
							
			if(prepare != undefined) {
				prepare(mesh);		
			}
			
			let interaction = new THREE.Interaction(renderInfo.renderer, renderInfo.scene, renderInfo.camera);
			render(
				renderInfo.renderer, renderInfo.scene, renderInfo.camera, 
				motion == undefined ? undefined : () => motion(mesh), cameraMotion
			);
			resolve(mesh);
		}, undefined, function (error) {
			reject(error);
		});
	});
}

function render(renderer, scene, camera, motion, cameraMotion) {
	requestAnimationFrame(
		() => render(renderer, scene, camera, motion, cameraMotion)
	);
	renderer.render(scene, camera);
	
	if(motion != undefined) {
		motion();
	}
	
	if(cameraMotion != undefined) {
		cameraMotion(camera);
		camera.updateProjectionMatrix();
	}
}

