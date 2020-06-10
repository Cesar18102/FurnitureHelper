function defaultFragment(colorEffect) {
	return `#define STANDARD
			#ifdef PHYSICAL
				#define REFLECTIVITY
				#define CLEARCOAT
				#define TRANSPARENCY
			#endif
			uniform float time;
			uniform bool timing;
			uniform vec3 mycolor;
			uniform bool outline;
			uniform vec3 outlineColor;
			uniform float outlineStrength;
			uniform sampler2D sampler;
			uniform vec2 textureSize;
			
			varying vec3 lightDir;
			varying vec2 v_texCoords;
			varying vec3 normalPass;
			
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
				` + getFragColor(colorEffect) +
				`
				#include <tonemapping_fragment>
				#include <encodings_fragment>
				#include <fog_fragment>
				#include <premultiplied_alpha_fragment>
				#include <dithering_fragment>
			}`;
}

function getFragColor(colorEffect) {
	let defaultColor = colorEffect ? 
		'vec4((outgoingLight.x + mycolor.x) / 2.0, (outgoingLight.y + mycolor.y) / 2.0, (outgoingLight.z + mycolor.z) / 2.0, diffuseColor.a)' : 
		'vec4(outgoingLight.x, outgoingLight.y, outgoingLight.z, diffuseColor.a)';
	
	/*return `
		if(!outline) {
			gl_FragColor = ` + defaultColor + `;
		} else {
			vec2 outlineSize = vec2(outlineStrength / textureSize.x, outlineStrength / textureSize.y);
			if(v_texCoords.x >= 1.0 - outlineSize.x || v_texCoords.x <= outlineSize.x || v_texCoords.y >= 1.0 - outlineSize.y || v_texCoords.y <= outlineSize.y) {
				gl_FragColor = vec4(outlineColor, 1.0);
			} else {
				gl_FragColor = ` + defaultColor + `;
			}
		}
	`;*/
	
	return `
		if(!outline) {
			gl_FragColor = ` + defaultColor + `;
		} else {
			float intensity = dot(lightDir, normalize(normalPass));	
			float factor = atan(intensity);
			if(timing) {
				factor *= sin(time * 2.0);
			}
			vec4 _color = vec4(outlineColor * factor, 1.0);
			gl_FragColor = vec4((outgoingLight.x + _color.x) / 2.0, (outgoingLight.y + _color.y) / 2.0, (outgoingLight.z + _color.z) / 2.0, diffuseColor.a);
		}
	`
}

function defaultVertex() {
	return `#define STANDARD
			varying vec3 vViewPosition;
			#ifndef FLAT_SHADED
				varying vec3 vNormal;
				#ifdef USE_TANGENT
					varying vec3 vTangent;
					varying vec3 vBitangent;
				#endif
			#endif
			uniform vec3 lightPos;
			varying vec3 lightDir;
			varying vec3 normalPass;
			varying vec2 v_texCoords;
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
				v_texCoords = uv;
				normalPass = normalize(normalMatrix * normal);
				lightDir = normalize(lightPos - (modelViewMatrix * vec4(position, 1.0)).xyz);
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