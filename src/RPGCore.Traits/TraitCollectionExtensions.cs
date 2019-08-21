using System.Collections.Generic;

namespace RPGCore.Traits
{
	public static class TraitCollectionExtensions
	{
		public static void SetTemplate<TStatInstance, TStateInstance, TStatTemplate, TStateTemplate>(
			this TraitCollection<TStatInstance, TStateInstance> instances, 
			TraitCollection<TStatTemplate, TStateTemplate> templates
		)
			where TStatTemplate : IFixedElement, new()
			where TStateTemplate : IFixedElement, new()
			where TStatInstance : ITemplatedElement<TStatTemplate>, IFixedElement, new()
			where TStateInstance : ITemplatedElement<TStateTemplate>, IFixedElement, new()
		{
			ParralelSet(instances.Stats, templates.Stats);
			ParralelSet(instances.States, templates.States);
		}

		private static void ParralelSet<TInstance, TTemplate>(IEnumerable<TInstance> instances,
			IEnumerable<TTemplate> templates)
			where TInstance : ITemplatedElement<TTemplate>
		{
			var instanceEnumerator = instances.GetEnumerator();
			var templateEnumerator = templates.GetEnumerator();

			while (instanceEnumerator.MoveNext() && templateEnumerator.MoveNext())
			{
				instanceEnumerator.Current.SetTemplate(templateEnumerator.Current);
			}
		}
	}
}
