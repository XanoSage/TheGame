using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class PlaneMeshGererator {
	
		private static Vector3[] vertices;
		private static Vector2[] uv;
		private static Vector3[] normals;
		private static int[] triangles;
		private static Mesh mesh;
		
		public static void Generate(Transform meshTransform, float width, float height, Vector3 normalLocal, Vector3 upLocal) {
			Generate(meshTransform, width, height,normalLocal,upLocal,Vector2.zero,Vector2.one);
		}
		
		public static void Generate(Transform meshTransform, float width, float height, Vector3 normalLocal, Vector3 upLocal, Vector2 leftBottomUV, Vector2 rightTopUV) {
			Generate(meshTransform, width, height, normalLocal, upLocal, leftBottomUV, rightTopUV, Vector3.zero);
		}
		public static void Generate(Transform meshTransform, float width, float height, Vector3 normalLocal, Vector3 upLocal, Vector3 localCenter) {
			Generate(meshTransform, width, height, normalLocal, upLocal, Vector2.zero,Vector2.one, localCenter);
		}
		public static void Generate(Transform meshTransform, float width, float height, Vector3 normalLocal, Vector3 upLocal, Vector2 leftBottomUV, Vector2 rightTopUV, Vector3 localCenter) {
			
		 	GameObject gameObject = meshTransform.gameObject;
			
			// Create mesh.
			mesh = new Mesh();
			MeshFilter meshComponent = gameObject.GetComponent<MeshFilter>();
			if (meshComponent == null)
				meshComponent = gameObject.AddComponent<MeshFilter>();
			meshComponent.mesh = mesh;
			
			// Create geometry.
			vertices = new Vector3[4];
			normals = new Vector3[4];
			uv = new Vector2[4];
			triangles = new int[6];
			
			Vector3 toSide = Vector3.Cross(normalLocal, upLocal).normalized*width*0.5f;
			Vector3 toUp = Vector3.Cross(toSide, normalLocal).normalized*height*0.5f;
			const float scale = 1;
			vertices[0] = (toSide-toUp)*scale+localCenter;
			vertices[1] = (-toSide-toUp)*scale+localCenter;
			vertices[2] = (-toSide+toUp)*scale+localCenter;
			vertices[3] = (toSide+toUp)*scale+localCenter;
			
			normals[0] = normalLocal;
			normals[1] = normalLocal;
			normals[2] = normalLocal;
			normals[3] = normalLocal;
		
			//uv[0] = new Vector2(1,0);
			//uv[1] = new Vector2(0,0);
			//uv[2] = new Vector2(0,1);
			//uv[3] = new Vector2(1,1);
			
			uv[0] = new Vector2(rightTopUV.x,leftBottomUV.y);
			uv[1] = new Vector2(leftBottomUV.x,leftBottomUV.y);
			uv[2] = new Vector2(leftBottomUV.x,rightTopUV.y);
			uv[3] = new Vector2(rightTopUV.x,rightTopUV.y);
		
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;
			triangles[3] = 0;
			triangles[4] = 2;
			triangles[5] = 3;
			
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.normals = normals;
			
			// Create renderer.
			if (meshTransform.gameObject.GetComponent<MeshRenderer>()==null)
				meshTransform.gameObject.AddComponent<MeshRenderer>();
			//ModelInitializer.SetTransparentColoredMaterial(meshTransform);
		}
		
		public static void Generate(Transform meshTransform, int XSections, int YSections, float width, float height, Vector3 normalLocal, Vector3 upLocal) {
			
		 	GameObject gameObject = meshTransform.gameObject;
			
			// Create mesh.
			mesh = new Mesh();
			MeshFilter meshComponent = gameObject.GetComponent<MeshFilter>();
			if (meshComponent == null)
				meshComponent = gameObject.AddComponent<MeshFilter>();
			meshComponent.mesh = mesh;
			
			// Create geometry.
			int vertexCount = (XSections+1)*(YSections+1);
			vertices = new Vector3[vertexCount];
			normals = new Vector3[vertexCount];
			uv = new Vector2[vertexCount];
			triangles = new int[XSections*YSections*2*3];
			
			Vector3 toSide = Vector3.Cross(normalLocal, upLocal).normalized*width*0.5f;
			Vector3 toUp = Vector3.Cross(toSide, normalLocal).normalized*height*0.5f;
			for (int y=0;y<YSections+1;y++) {
				for (int x=0;x<XSections+1;x++) {
					int shift = (y*(XSections+1)+x);
					vertices[shift] = toSide*(x-(XSections)*0.5f)/(XSections)+toUp*(y-(YSections)*0.5f)/(YSections);
					
					normals[shift] = normalLocal;
					
					/*uv[shift+0] = new Vector2(rightTopUV.x,leftBottomUV.y);
					uv[shift+1] = new Vector2(leftBottomUV.x,leftBottomUV.y);
					uv[shift+2] = new Vector2(leftBottomUV.x,rightTopUV.y);
					uv[shift+3] = new Vector2(rightTopUV.x,rightTopUV.y);*/
				}
			}
			
			for (int y=0;y<YSections;y++) {
				for (int x=0;x<XSections;x++) {
					int shift = (y*XSections+x)*6;
					triangles[shift+0] = x+1+y*(XSections+1);
					triangles[shift+1] = x+y*(XSections+1);
					triangles[shift+2] = x+(y+1)*(XSections+1);
					triangles[shift+3] = triangles[shift+0];
					triangles[shift+4] = triangles[shift+2];
					triangles[shift+5] = x+1+(y+1)*(XSections+1);
				}
			}
			
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.normals = normals;
			
			// Create renderer.
			if (meshTransform.gameObject.GetComponent<MeshRenderer>()==null)
				meshTransform.gameObject.AddComponent<MeshRenderer>();
			//ModelInitializer.SetTransparentColoredMaterial(meshTransform);
		}
	}
}