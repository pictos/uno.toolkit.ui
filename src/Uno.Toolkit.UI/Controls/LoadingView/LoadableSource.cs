﻿using System;
using System.Threading;
using Uno.Disposables;
using Windows.System;

#if IS_WINUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Toolkit.UI;

/// <summary>
/// Represents an <see cref="ILoadable" /> that forwards the <see cref="ILoadable.IsExecuting"/> state of its <see cref="Source" />.
/// </summary>
public partial class LoadableSource : FrameworkElement, ILoadable
{
	public event EventHandler? IsExecutingChanged;

	private DispatcherQueue _dispatcherQueue = Windows.System.DispatcherQueue.GetForCurrentThread();

	#region DependencyProperty: Source

	public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(
		nameof(Source),
		typeof(ILoadable),
		typeof(LoadableSource),
		new PropertyMetadata(default(ILoadable), (s, e) => ((LoadableSource)s).OnSourceChanged(e)));

	/// <summary>
	/// Gets and sets the <see cref="ILoadable" /> to forward its state.
	/// </summary>
	public ILoadable Source
	{
		get => (ILoadable)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	#endregion
	#region DependencyProperty: IsExecuting

	public static DependencyProperty IsExecutingProperty { get; } = DependencyProperty.Register(
		nameof(IsExecuting),
		typeof(bool),
		typeof(LoadableSource),
		new PropertyMetadata(default(bool), (s, e) => ((LoadableSource)s).OnIsExecutingChanged(e)));

	public bool IsExecuting
	{
		get => (bool)GetValue(IsExecutingProperty);
		set => SetValue(IsExecutingProperty, value);
	}

	#endregion

	private readonly SerialDisposable _subscription = new();

	private void OnIsExecutingChanged(DependencyPropertyChangedEventArgs e)
	{
		IsExecutingChanged?.Invoke(this, new());
	}

	private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
	{
		var source = Source;

		void Update() => IsExecuting = Source.IsExecuting;
		void UpdateOnDispatcher()
		{
			_ = _dispatcherQueue.ExecuteAsync(async (cancellation) => Update(), CancellationToken.None);
		}

		_subscription.Disposable = source?.BindIsExecuting(UpdateOnDispatcher, propagateInitialValue: false);
		IsExecuting = source?.IsExecuting ?? false;
	}
}
