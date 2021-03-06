#region 脚本说明
/*----------------------------------------------------------------
// 脚本作用：特效优化检查工具2.0
// 创建者：黑仔
//----------------------------------------------------------------*/
#endregion
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EffectJianCha : EditorWindow 
{

	//-------------------特效预览---------------------
	private List<int> psCounts = new List<int>();
	private List<int> meshTrians = new List<int>();
	private List<Material> matCounts = new List<Material>();
	private List<Texture> texCounts = new List<Texture>();
	private List<GameObject> objCounts = new List<GameObject>();

	private static bool openPreview = false;
	private bool zaiRu = false;
	private GameObject dqAsset;
	private GameObject dqEffect;
	private GameObject dqPrefab;
	/// <summary>
	/// 上次选择的预设
	/// </summary>
	private GameObject scSPrefab;

	private int shiLieHua = 0;

	/// <summary>
	/// 最大粒子数
	/// </summary>
	private int psCount = 0;

	/// <summary>
	/// 最大模型面数
	/// </summary>
	private int meshTrian = 0;

	/// <summary>
	/// 发射mesh的ParticleSystem数
	/// </summary>
	private int meshPSCount = 0;

	/// <summary>
	/// 播放模式下时间控制
	/// </summary>
	private float playingTime = 0.0f;
	private float runPlayingTime = 0.0f;
	private bool isPlayingTime = false;

	private Vector2 scrollPos = new Vector2();

	//-----------------------------------------------------

	[MenuItem("HZTools/特效优化检查")]
	static void AddArtGongJu01()
	{
		openPreview = false;
		GetWindow<EffectJianCha>("特效检查情况显示窗口");
	}

	void OnGUI()
	{
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.Toggle("开启资源预览检查特效", openPreview);
		if (EditorGUI.EndChangeCheck())
		{
			psCount = 0;
			meshTrian = 0;
			meshPSCount = 0;
			openPreview = !openPreview;
			if (!openPreview) 
			{
				if (dqEffect != null && shiLieHua > 0) 
				{
					DestroyImmediate(dqEffect);
					zaiRu = false;
				}
				else {
					dqEffect = null;
					zaiRu = false;
				}

				ListClear();
			}
			dqPrefab = null;
			OnSelectionChange();
		}

		EditorGUILayout.LabelField("当前特效的最大粒子数： " + psCount);
		EditorGUILayout.LabelField("当前特效大概的三角面数： " + ((meshTrian / 3) + (psCount * 2)).ToString());
		EditorGUILayout.LabelField("当前特效发射mesh的ParticleSystem数： " + meshPSCount);
		EditorGUILayout.Space();
		EditorGUILayout.ObjectField("上次选择的预设： ", scSPrefab, typeof(GameObject), false);
		EditorGUILayout.LabelField("当前特效的材质数量： " + matCounts.Count);
		EditorGUILayout.LabelField("当前特效的贴图数量： " + texCounts.Count);
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		if (objCounts.Count > 0) 
		{
			foreach (var obj in objCounts)
			{
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical(GUILayout.Width(200));
				EditorGUILayout.ObjectField(obj, typeof(GameObject), false, GUILayout.Width(200));
				if (obj) 
				{
					var ps = obj.GetComponent<ParticleSystem>();
					if (ps) 
					{
						if (ps.GetComponent<ParticleSystemRenderer>().renderMode == ParticleSystemRenderMode.Mesh && ps.GetComponent<ParticleSystemRenderer>().mesh != null) 
						{
							EditorGUILayout.LabelField("PS.Mesh的面数: " + (ps.GetComponent<ParticleSystemRenderer>().mesh.triangles.Length / 3));
						}
					}
					var meshF = obj.GetComponent<MeshFilter>();
					if (meshF) 
					{
						if (meshF.sharedMesh) 
						{
							EditorGUILayout.LabelField("Mesh的面数: " + (meshF.sharedMesh.triangles.Length / 3));
						}
					}
					var meshSkinF = obj.GetComponent<SkinnedMeshRenderer>();
					if (meshSkinF) 
					{
						if (meshSkinF.sharedMesh) 
						{
							EditorGUILayout.LabelField("Mesh的面数: " + (meshSkinF.sharedMesh.triangles.Length / 3));
						}
					}
				}
				GUILayout.EndVertical();
				if (obj != null) 
				{
					var rend = obj.GetComponent<Renderer>();
					var matS = rend.sharedMaterials;
					if (matS.Length > 0) 
					{
						foreach (var mat in matS) 
						{
							if (mat != null) 
							{
								var matLB = (Material)EditorGUILayout.ObjectField(mat, typeof(Material), false, GUILayout.Width(200));
								int count = ShaderUtil.GetPropertyCount(mat.shader);
								for (int i = 0; i < count; i++) 
								{
									var matShaderType = ShaderUtil.GetPropertyType(mat.shader, i);
									if (ShaderUtil.ShaderPropertyType.TexEnv == matShaderType) 
									{
										var assetMatShaderProName = ShaderUtil.GetPropertyName(mat.shader, i);
										var tex = mat.GetTexture(assetMatShaderProName);
										if (tex != null) 
										{
											tex = (Texture)EditorGUILayout.ObjectField("像素: " + tex.height + "*" + tex.width, tex, typeof(Texture), false, GUILayout.Width(200));
										}
									}
								}
							}
						}
					}
				}

				GUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
		}
		EditorGUILayout.EndScrollView();
	}

	//------------------------特效预览------------------------------------------

	void OnDestroy()
	{
		openPreview = false;
		if (dqEffect != null) 
		{
			DestroyImmediate(dqEffect);
			zaiRu = false;
		}
	}

	void ListClear()
	{
		meshTrians.Clear();
		psCounts.Clear();
		meshPSCount = 0;
		matCounts.Clear();
		texCounts.Clear();
		objCounts.Clear();
	}

	void YouHuaJianCha(GameObject go)
	{
		ListClear();

		var objTranS = go.GetComponentsInChildren<Transform>();
		if (objTranS.Length > 0) 
		{
			foreach (var objTran in objTranS) 
			{
				var obj = objTran.gameObject;
				var rend = obj.GetComponent<Renderer>();
				if (rend != null) 
				{
					if (!objCounts.Contains(obj)) 
					{
						objCounts.Add(obj);
					}
				}
			}
		}

		var meshs = go.GetComponentsInChildren<MeshFilter>();
		foreach (var mesh in meshs) 
		{
			if (mesh.sharedMesh != null) 
			{
				meshTrians.Add(mesh.sharedMesh.triangles.Length);
			} 
		}

		var psS = go.GetComponentsInChildren<ParticleSystem>();
		foreach (var ps in psS) 
		{
			var psConut = ps.GetParticles(new ParticleSystem.Particle[ps.maxParticles]);
			if (ps.GetComponent<ParticleSystemRenderer>().renderMode == ParticleSystemRenderMode.Mesh && ps.GetComponent<ParticleSystemRenderer>().mesh != null) 
			{
				meshTrians.Add(ps.GetComponent<ParticleSystemRenderer>().mesh.triangles.Length * psConut);
				meshPSCount = meshPSCount + 1;
			}
			var psNum = ps.GetParticles(new ParticleSystem.Particle[ps.maxParticles]);
			psCounts.Add(psNum);
		}
		var skinMeshs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (var mesh in skinMeshs) 
		{
			if (mesh.sharedMesh != null) 
			{
				meshTrians.Add(mesh.sharedMesh.triangles.Length);
			}
		}

		var rendS = go.GetComponentsInChildren<Renderer>();
		if (rendS.Length > 0) 
		{
			foreach (var rend in rendS) 
			{
				var matS = rend.sharedMaterials;
				if (matS.Length > 0) 
				{
					foreach (var mat in matS) 
					{
						if (mat != null) 
						{
							if (!matCounts.Contains(mat)) 
							{
								matCounts.Add(mat);
							}
						} else {
							Debug.LogError("这个预设有缺少材质！！！！！！！");
						}
					}
				}
			}
		}

		if (matCounts.Count > 0) 
		{
			foreach (var mat in matCounts) 
			{
				int count = ShaderUtil.GetPropertyCount(mat.shader);
				for (int i = 0; i < count; i++) 
				{
					var matShaderType = ShaderUtil.GetPropertyType(mat.shader, i);
					if (ShaderUtil.ShaderPropertyType.TexEnv == matShaderType) 
					{
						var assetMatShaderProName = ShaderUtil.GetPropertyName(mat.shader, i);
						var tex = mat.GetTexture(assetMatShaderProName);
						if (tex != null) 
						{
							//查找list里是否存在某个对象
							if (!texCounts.Contains(tex)) 
							{
								texCounts.Add(tex);
							}
						}
					}
				}
			}
		}


		if (meshTrians.Count > 0) 
		{
			var psNum = 0;
			foreach (var item in meshTrians) 
			{
				psNum = psNum + item;
			}
			if (meshTrian < psNum) 
			{
				meshTrian = psNum;
			}
		}


		if (psCounts.Count > 0) 
		{
			var psNum = 0;
			foreach (var item in psCounts) 
			{
				psNum = psNum + item;
			}
			if (psCount < psNum) 
			{
				psCount = psNum;
			}
		}
		Repaint();
	}

	void Update()
	{
		GetdqPrefab();

		if (dqEffect != null) 
		{
			YouHuaJianCha(dqEffect);
		}

		if (dqPrefab != null && !isPlayingTime && playingTime > 0) 
		{
			runPlayingTime = playingTime;
			if (!isPlayingTime && dqPrefab!= null && runPlayingTime > 0.0f) 
			{
				PlayingAllAniPlay();
			}
		}
		if (isPlayingTime) 
		{
			var _time = Time.deltaTime;
			if (_time <= 0.02f) 
			{
				_time = 0.02f;
			}
			runPlayingTime = runPlayingTime - _time;
			if (runPlayingTime < 0) 
			{
				isPlayingTime = false;
				playingTime = 0;
			}
		}

		GetPlayingTime();
	}

	void OnSelectionChange()
	{
		var selObj = Selection.activeObject;
		if (selObj != null && selObj.GetType() != typeof(GameObject)) 
		{
			return;
		}

		if (openPreview && shiLieHua > 0) 
		{
			DestroyImmediate(dqEffect);
			zaiRu = false;
		}

		if (openPreview) 
		{
			dqAsset = Selection.activeGameObject;
			if (openPreview && dqAsset != null && dqAsset.transform.root == dqAsset.transform)
			{
				scSPrefab = dqAsset;
			}
			JiaZaiEffect();
		}
	}

	void JiaZaiEffect()
	{
		if (openPreview && dqAsset != null && dqAsset.transform.root == dqAsset.transform) 
		{
			if (AssetDatabase.Contains(dqAsset)) 
			{
				var go = Instantiate(dqAsset, new Vector3(0,0,0), dqAsset.transform.localRotation);
				dqEffect = go as GameObject;
				shiLieHua += 1;
				zaiRu = true;
			} else {
				dqEffect = dqAsset;
				shiLieHua = 0;
				zaiRu = false;
			}

			psCount = 0;
			meshTrian = 0;
			playingTime = 0.0f;
		}
	}
		

	//---------------------------------------------------------------------------------------------

	//------------------------------------播放模式下循环播放------------------------------------------
	void GetdqPrefab()
	{
		if (openPreview && dqEffect != null && dqAsset != null) 
		{
			dqPrefab = dqEffect;
		}
	}

	void GetPlayingTime()
	{
		if (dqPrefab != null && playingTime <= 0) 
		{
			var psS = dqPrefab.GetComponentsInChildren<ParticleSystem>();
			if (psS.Length > 0) 
			{
				foreach (var ps in psS) 
				{
					if ((ps.duration + ps.startLifetime + ps.startDelay) > playingTime) 
					{
						playingTime = ps.duration + ps.startLifetime + ps.startDelay;
					}
				}
			}

			var atrS = dqPrefab.GetComponentsInChildren<Animator>();
			if (atrS.Length > 0) 
			{
				foreach (var atr in atrS) 
				{
					if (playingTime < atr.GetCurrentAnimatorStateInfo(0).length) 
					{
						playingTime = atr.GetCurrentAnimatorStateInfo(0).length;
					}
				}
			}

			var aniS = dqPrefab.GetComponentsInChildren<Animation>();
			if (aniS.Length > 0) 
			{
				foreach (var ani in aniS) 
				{
					if (ani.clip.length > playingTime) 
					{
						playingTime = ani.clip.length; 
					}
				}
			}
			isPlayingTime = false;
			runPlayingTime = playingTime;
		}
	}

	void PlayingAllAniPlay()
	{
		if (dqPrefab != null) 
		{
			var psS = dqPrefab.GetComponentsInChildren<ParticleSystem>();
			if (psS.Length > 0) 
			{
				foreach (var ps in psS) 
				{
					if (!ps.isPlaying) 
					{
						ps.Play(true);
					}
				}
			}
			
			var atrS = dqPrefab.GetComponentsInChildren<Animator>();
			if (atrS.Length > 0) 
			{
				foreach (var atr in atrS) 
				{
					var hash = atr.GetCurrentAnimatorStateInfo(0).fullPathHash;
					if (atr.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) 
					{
						atr.Play(hash, 0, 0);
					}
				}
			}
			
			var aniS = dqPrefab.GetComponentsInChildren<Animation>();
			if (aniS.Length > 0) 
			{
				foreach (var ani in aniS) 
				{
					if (!ani.isPlaying) 
					{
						ani.Play();
					}
				}
			}
			isPlayingTime = true;
		}
	}
	//-----------------------------------------------------------------------------------------------
}