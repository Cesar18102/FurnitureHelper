let OBJLoader = new THREE.OBJLoader();

export function renderPart(part, renderInfo, prepare, motion) {
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
			
			let material = new THREE.MeshPhysicalMaterial({ 
				map : texture, 
				flatShading : true
			});
							
			material.onBeforeCompile = shader => {
				if(part.color != undefined) {
					shader.uniforms.mycolor = { value : new THREE.Color(red * alpha, green * alpha, blue * alpha) };
				}
								
				shader.vertexShader = vertex();
				shader.fragmentShader = fragment(part.color != undefined);
			};
							
			let geometry = object.children[0].geometry;
			let mesh = new THREE.Mesh(geometry, material);
			renderInfo.scene.add(mesh);
							
			if(prepare != undefined) {
				prepare(mesh.geometry);		
			}		
			
			render(renderInfo.renderer, renderInfo.scene, renderInfo.camera, motion == undefined ? undefined : () => motion(mesh));
			resolve(mesh);
		}, undefined, function (error) {
			reject(error);
		});
	});
}

function render(renderer, scene, camera, motion) {
	requestAnimationFrame(() => render(renderer, scene, camera, motion));
	renderer.render(scene, camera);
	
	if(motion != undefined) {
		motion();
	}
}

function fragment(colorEffect) {//uniform sampler2D sampler;
	return `#define STANDARD
			#ifdef PHYSICAL
				#define REFLECTIVITY
				#define CLEARCOAT
				#define TRANSPARENCY
			#endif
			uniform vec3 mycolor;
			
			uniform vec3 diffuse;
			uniform vec3 emissive;
			uniform float roughness;
			uniform float metalness;
			uniform float opacity;
			#ifdef TRANSPARENCY
				uniform float transparency;
			#endif
			#ifdef REFLECTIVITY
				uniform float reflectivity;
			#endif
			#ifdef CLEARCOAT
				uniform float clearcoat;
				uniform float clearcoatRoughness;
			#endif
			#ifdef USE_SHEEN
				uniform vec3 sheen;
			#endif
			varying vec2 point;
			varying vec3 vViewPosition;
			#ifndef FLAT_SHADED
				varying vec3 vNormal;
				#ifdef USE_TANGENT
					varying vec3 vTangent;
					varying vec3 vBitangent;
				#endif
			#endif
			#include <common>
			#include <packing>
			#include <dithering_pars_fragment>
			#include <color_pars_fragment>
			#include <uv_pars_fragment>
			#include <uv2_pars_fragment>
			#include <map_pars_fragment>
			#include <alphamap_pars_fragment>
			#include <aomap_pars_fragment>
			#include <lightmap_pars_fragment>
			#include <emissivemap_pars_fragment>
			#include <bsdfs>
			#include <cube_uv_reflection_fragment>
			#include <envmap_common_pars_fragment>
			#include <envmap_physical_pars_fragment>
			#include <fog_pars_fragment>
			#include <lights_pars_begin>
			#include <lights_physical_pars_fragment>
			#include <shadowmap_pars_fragment>
			#include <bumpmap_pars_fragment>
			#include <normalmap_pars_fragment>
			#include <clearcoat_pars_fragment>
			#include <roughnessmap_pars_fragment>
			#include <metalnessmap_pars_fragment>
			#include <logdepthbuf_pars_fragment>
			#include <clipping_planes_pars_fragment>
			void main() {
				#include <clipping_planes_fragment>
				vec4 diffuseColor = vec4( diffuse, opacity );
				ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
				vec3 totalEmissiveRadiance = emissive;
				#include <logdepthbuf_fragment>
				#include <map_fragment>
				#include <color_fragment>
				#include <alphamap_fragment>
				#include <alphatest_fragment>
				#include <roughnessmap_fragment>
				#include <metalnessmap_fragment>
				#include <normal_fragment_begin>
				#include <normal_fragment_maps>
				#include <clearcoat_normal_fragment_begin>
				#include <clearcoat_normal_fragment_maps>
				#include <emissivemap_fragment>
				#include <lights_physical_fragment>
				#include <lights_fragment_begin>
				#include <lights_fragment_maps>
				#include <lights_fragment_end>
				#include <aomap_fragment>
				vec3 outgoingLight = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse + reflectedLight.directSpecular + reflectedLight.indirectSpecular + totalEmissiveRadiance;
				#ifdef TRANSPARENCY
					diffuseColor.a *= saturate( 1. - transparency + linearToRelativeLuminance( reflectedLight.directSpecular + reflectedLight.indirectSpecular ) );
				#endif
				` + 
				(colorEffect ?
				'gl_FragColor = vec4((outgoingLight.x + mycolor.x) / 2.0, (outgoingLight.y + mycolor.y) / 2.0, (outgoingLight.z + mycolor.z) / 2.0, diffuseColor.a);' : 
				'gl_FragColor = vec4(outgoingLight.x, outgoingLight.y, outgoingLight.z, diffuseColor.a);') +
				`
				#include <tonemapping_fragment>
				#include <encodings_fragment>
				#include <fog_fragment>
				#include <premultiplied_alpha_fragment>
				#include <dithering_fragment>
			}`;
}

function vertex() {
	return `#define STANDARD
			varying vec2 point;
			varying vec3 vViewPosition;
			#ifndef FLAT_SHADED
				varying vec3 vNormal;
				#ifdef USE_TANGENT
					varying vec3 vTangent;
					varying vec3 vBitangent;
				#endif
			#endif
			#include <common>
			#include <uv_pars_vertex>
			#include <uv2_pars_vertex>
			#include <displacementmap_pars_vertex>
			#include <color_pars_vertex>
			#include <fog_pars_vertex>
			#include <morphtarget_pars_vertex>
			#include <skinning_pars_vertex>
			#include <shadowmap_pars_vertex>
			#include <logdepthbuf_pars_vertex>
			#include <clipping_planes_pars_vertex>
			void main() {
				point = uv;
				#include <uv_vertex>
				#include <uv2_vertex>
				#include <color_vertex>
				#include <beginnormal_vertex>
				#include <morphnormal_vertex>
				#include <skinbase_vertex>
				#include <skinnormal_vertex>
				#include <defaultnormal_vertex>
			#ifndef FLAT_SHADED
				vNormal = normalize( transformedNormal );
				#ifdef USE_TANGENT
					vTangent = normalize( transformedTangent );
					vBitangent = normalize( cross( vNormal, vTangent ) * tangent.w );
				#endif
			#endif
				#include <begin_vertex>
				#include <morphtarget_vertex>
				#include <skinning_vertex>
				#include <displacementmap_vertex>
				#include <project_vertex>
				#include <logdepthbuf_vertex>
				#include <clipping_planes_vertex>
				vViewPosition = - mvPosition.xyz;
				#include <worldpos_vertex>
				#include <shadowmap_vertex>
				#include <fog_vertex>
			}`;
}