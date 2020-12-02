using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Projects
{
	public sealed class BuildActionCollection : BuildAction, IEnumerable<BuildAction>
	{
		private readonly List<BuildAction> buildActions;

		public BuildActionCollection()
		{
			buildActions = new List<BuildAction>();
		}

		public BuildActionCollection(List<BuildAction> buildActions)
		{
			this.buildActions = buildActions ?? throw new ArgumentNullException(nameof(buildActions));
		}

		public override void OnBeforeExportResource(ProjectBuildProcess process, ProjectResource resource)
		{
			foreach (var buildAction in buildActions)
			{
				buildAction.OnBeforeExportResource(process, resource);
			}
		}

		public override void OnAfterExportResource(ProjectBuildProcess process, ProjectResource resource)
		{
			foreach (var buildAction in buildActions)
			{
				buildAction.OnAfterExportResource(process, resource);
			}
		}

		public void Add(BuildAction action)
		{
			buildActions.Add(action);
		}

		public IEnumerator<BuildAction> GetEnumerator()
		{
			return buildActions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
