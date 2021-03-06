﻿using System;
using System.IO;
using System.Text;
using uTinyRipper.Classes;
using uTinyRipper.Classes.Shaders;
using uTinyRipper.Classes.Shaders.Exporters;

namespace uTinyRipper
{
	public class ShaderWriter : InvariantStreamWriter
	{
		public ShaderWriter(Stream stream, Shader shader, Func<Version, ShaderGpuProgramType, ShaderTextExporter> exporterInstantiator) :
			base(stream, new UTF8Encoding(false), 4096, true)
		{
			if(shader == null)
			{
				throw new ArgumentNullException(nameof(shader));
			}
			if(exporterInstantiator == null)
			{
				throw new ArgumentNullException(nameof(exporterInstantiator));
			}

			Shader = shader;
			m_exporterInstantiator = exporterInstantiator;
		}

		public void WriteShaderData(ShaderGpuProgramType programType, byte[] shaderData)
		{
			ShaderTextExporter exporter = m_exporterInstantiator.Invoke(Shader.File.Version, programType);
			exporter.Export(shaderData, this);
		}
		
		public Shader Shader { get; }
		public Version Version => Shader.File.Version;
		public Platform Platform => Shader.File.Platform;

		private readonly Func<Version, ShaderGpuProgramType, ShaderTextExporter> m_exporterInstantiator;
	}
}
