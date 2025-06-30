var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i2580 = root || request.c( 'UnityEngine.JointSpring' )
  var i2581 = data
  i2580.spring = i2581[0]
  i2580.damper = i2581[1]
  i2580.targetPosition = i2581[2]
  return i2580
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i2582 = root || request.c( 'UnityEngine.JointMotor' )
  var i2583 = data
  i2582.m_TargetVelocity = i2583[0]
  i2582.m_Force = i2583[1]
  i2582.m_FreeSpin = i2583[2]
  return i2582
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i2584 = root || request.c( 'UnityEngine.JointLimits' )
  var i2585 = data
  i2584.m_Min = i2585[0]
  i2584.m_Max = i2585[1]
  i2584.m_Bounciness = i2585[2]
  i2584.m_BounceMinVelocity = i2585[3]
  i2584.m_ContactDistance = i2585[4]
  i2584.minBounce = i2585[5]
  i2584.maxBounce = i2585[6]
  return i2584
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i2586 = root || request.c( 'UnityEngine.JointDrive' )
  var i2587 = data
  i2586.m_PositionSpring = i2587[0]
  i2586.m_PositionDamper = i2587[1]
  i2586.m_MaximumForce = i2587[2]
  i2586.m_UseAcceleration = i2587[3]
  return i2586
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i2588 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i2589 = data
  i2588.m_Spring = i2589[0]
  i2588.m_Damper = i2589[1]
  return i2588
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i2590 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i2591 = data
  i2590.m_Limit = i2591[0]
  i2590.m_Bounciness = i2591[1]
  i2590.m_ContactDistance = i2591[2]
  return i2590
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i2592 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i2593 = data
  i2592.m_ExtremumSlip = i2593[0]
  i2592.m_ExtremumValue = i2593[1]
  i2592.m_AsymptoteSlip = i2593[2]
  i2592.m_AsymptoteValue = i2593[3]
  i2592.m_Stiffness = i2593[4]
  return i2592
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i2594 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i2595 = data
  i2594.m_LowerAngle = i2595[0]
  i2594.m_UpperAngle = i2595[1]
  return i2594
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i2596 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i2597 = data
  i2596.m_MotorSpeed = i2597[0]
  i2596.m_MaximumMotorTorque = i2597[1]
  return i2596
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i2598 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i2599 = data
  i2598.m_DampingRatio = i2599[0]
  i2598.m_Frequency = i2599[1]
  i2598.m_Angle = i2599[2]
  return i2598
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i2600 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i2601 = data
  i2600.m_LowerTranslation = i2601[0]
  i2600.m_UpperTranslation = i2601[1]
  return i2600
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i2602 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i2603 = data
  i2602.name = i2603[0]
  i2602.width = i2603[1]
  i2602.height = i2603[2]
  i2602.mipmapCount = i2603[3]
  i2602.anisoLevel = i2603[4]
  i2602.filterMode = i2603[5]
  i2602.hdr = !!i2603[6]
  i2602.format = i2603[7]
  i2602.wrapMode = i2603[8]
  i2602.alphaIsTransparency = !!i2603[9]
  i2602.alphaSource = i2603[10]
  i2602.graphicsFormat = i2603[11]
  i2602.sRGBTexture = !!i2603[12]
  i2602.desiredColorSpace = i2603[13]
  i2602.wrapU = i2603[14]
  i2602.wrapV = i2603[15]
  return i2602
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.RectTransform"] = function (request, data, root) {
  var i2604 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.RectTransform' )
  var i2605 = data
  i2604.pivot = new pc.Vec2( i2605[0], i2605[1] )
  i2604.anchorMin = new pc.Vec2( i2605[2], i2605[3] )
  i2604.anchorMax = new pc.Vec2( i2605[4], i2605[5] )
  i2604.sizeDelta = new pc.Vec2( i2605[6], i2605[7] )
  i2604.anchoredPosition3D = new pc.Vec3( i2605[8], i2605[9], i2605[10] )
  i2604.rotation = new pc.Quat(i2605[11], i2605[12], i2605[13], i2605[14])
  i2604.scale = new pc.Vec3( i2605[15], i2605[16], i2605[17] )
  return i2604
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasGroup"] = function (request, data, root) {
  var i2606 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasGroup' )
  var i2607 = data
  i2606.m_Alpha = i2607[0]
  i2606.m_Interactable = !!i2607[1]
  i2606.m_BlocksRaycasts = !!i2607[2]
  i2606.m_IgnoreParentGroups = !!i2607[3]
  i2606.enabled = !!i2607[4]
  return i2606
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Canvas"] = function (request, data, root) {
  var i2608 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Canvas' )
  var i2609 = data
  i2608.enabled = !!i2609[0]
  i2608.planeDistance = i2609[1]
  i2608.referencePixelsPerUnit = i2609[2]
  i2608.isFallbackOverlay = !!i2609[3]
  i2608.renderMode = i2609[4]
  i2608.renderOrder = i2609[5]
  i2608.sortingLayerName = i2609[6]
  i2608.sortingOrder = i2609[7]
  i2608.scaleFactor = i2609[8]
  request.r(i2609[9], i2609[10], 0, i2608, 'worldCamera')
  i2608.overrideSorting = !!i2609[11]
  i2608.pixelPerfect = !!i2609[12]
  i2608.targetDisplay = i2609[13]
  i2608.overridePixelPerfect = !!i2609[14]
  return i2608
}

Deserializers["UnityEngine.UI.GraphicRaycaster"] = function (request, data, root) {
  var i2610 = root || request.c( 'UnityEngine.UI.GraphicRaycaster' )
  var i2611 = data
  i2610.m_IgnoreReversedGraphics = !!i2611[0]
  i2610.m_BlockingObjects = i2611[1]
  i2610.m_BlockingMask = UnityEngine.LayerMask.FromIntegerValue( i2611[2] )
  return i2610
}

Deserializers["EndGameUI"] = function (request, data, root) {
  var i2612 = root || request.c( 'EndGameUI' )
  var i2613 = data
  request.r(i2613[0], i2613[1], 0, i2612, 'replayButton')
  i2612.maxScale = new pc.Vec3( i2613[2], i2613[3], i2613[4] )
  i2612.minScale = new pc.Vec3( i2613[5], i2613[6], i2613[7] )
  i2612.scaleDuration = i2613[8]
  return i2612
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer"] = function (request, data, root) {
  var i2614 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer' )
  var i2615 = data
  i2614.cullTransparentMesh = !!i2615[0]
  return i2614
}

Deserializers["UnityEngine.UI.Image"] = function (request, data, root) {
  var i2616 = root || request.c( 'UnityEngine.UI.Image' )
  var i2617 = data
  request.r(i2617[0], i2617[1], 0, i2616, 'm_Sprite')
  i2616.m_Type = i2617[2]
  i2616.m_PreserveAspect = !!i2617[3]
  i2616.m_FillCenter = !!i2617[4]
  i2616.m_FillMethod = i2617[5]
  i2616.m_FillAmount = i2617[6]
  i2616.m_FillClockwise = !!i2617[7]
  i2616.m_FillOrigin = i2617[8]
  i2616.m_UseSpriteMesh = !!i2617[9]
  i2616.m_PixelsPerUnitMultiplier = i2617[10]
  request.r(i2617[11], i2617[12], 0, i2616, 'm_Material')
  i2616.m_Maskable = !!i2617[13]
  i2616.m_Color = new pc.Color(i2617[14], i2617[15], i2617[16], i2617[17])
  i2616.m_RaycastTarget = !!i2617[18]
  i2616.m_RaycastPadding = new pc.Vec4( i2617[19], i2617[20], i2617[21], i2617[22] )
  return i2616
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i2618 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i2619 = data
  i2618.name = i2619[0]
  i2618.tagId = i2619[1]
  i2618.enabled = !!i2619[2]
  i2618.isStatic = !!i2619[3]
  i2618.layer = i2619[4]
  return i2618
}

Deserializers["UnityEngine.UI.Text"] = function (request, data, root) {
  var i2620 = root || request.c( 'UnityEngine.UI.Text' )
  var i2621 = data
  i2620.m_FontData = request.d('UnityEngine.UI.FontData', i2621[0], i2620.m_FontData)
  i2620.m_Text = i2621[1]
  request.r(i2621[2], i2621[3], 0, i2620, 'm_Material')
  i2620.m_Maskable = !!i2621[4]
  i2620.m_Color = new pc.Color(i2621[5], i2621[6], i2621[7], i2621[8])
  i2620.m_RaycastTarget = !!i2621[9]
  i2620.m_RaycastPadding = new pc.Vec4( i2621[10], i2621[11], i2621[12], i2621[13] )
  return i2620
}

Deserializers["UnityEngine.UI.FontData"] = function (request, data, root) {
  var i2622 = root || request.c( 'UnityEngine.UI.FontData' )
  var i2623 = data
  request.r(i2623[0], i2623[1], 0, i2622, 'm_Font')
  i2622.m_FontSize = i2623[2]
  i2622.m_FontStyle = i2623[3]
  i2622.m_BestFit = !!i2623[4]
  i2622.m_MinSize = i2623[5]
  i2622.m_MaxSize = i2623[6]
  i2622.m_Alignment = i2623[7]
  i2622.m_AlignByGeometry = !!i2623[8]
  i2622.m_RichText = !!i2623[9]
  i2622.m_HorizontalOverflow = i2623[10]
  i2622.m_VerticalOverflow = i2623[11]
  i2622.m_LineSpacing = i2623[12]
  return i2622
}

Deserializers["UnityEngine.UI.Button"] = function (request, data, root) {
  var i2624 = root || request.c( 'UnityEngine.UI.Button' )
  var i2625 = data
  i2624.m_OnClick = request.d('UnityEngine.UI.Button+ButtonClickedEvent', i2625[0], i2624.m_OnClick)
  i2624.m_Navigation = request.d('UnityEngine.UI.Navigation', i2625[1], i2624.m_Navigation)
  i2624.m_Transition = i2625[2]
  i2624.m_Colors = request.d('UnityEngine.UI.ColorBlock', i2625[3], i2624.m_Colors)
  i2624.m_SpriteState = request.d('UnityEngine.UI.SpriteState', i2625[4], i2624.m_SpriteState)
  i2624.m_AnimationTriggers = request.d('UnityEngine.UI.AnimationTriggers', i2625[5], i2624.m_AnimationTriggers)
  i2624.m_Interactable = !!i2625[6]
  request.r(i2625[7], i2625[8], 0, i2624, 'm_TargetGraphic')
  return i2624
}

Deserializers["UnityEngine.UI.Button+ButtonClickedEvent"] = function (request, data, root) {
  var i2626 = root || request.c( 'UnityEngine.UI.Button+ButtonClickedEvent' )
  var i2627 = data
  i2626.m_PersistentCalls = request.d('UnityEngine.Events.PersistentCallGroup', i2627[0], i2626.m_PersistentCalls)
  return i2626
}

Deserializers["UnityEngine.Events.PersistentCallGroup"] = function (request, data, root) {
  var i2628 = root || request.c( 'UnityEngine.Events.PersistentCallGroup' )
  var i2629 = data
  var i2631 = i2629[0]
  var i2630 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Events.PersistentCall')))
  for(var i = 0; i < i2631.length; i += 1) {
    i2630.add(request.d('UnityEngine.Events.PersistentCall', i2631[i + 0]));
  }
  i2628.m_Calls = i2630
  return i2628
}

Deserializers["UnityEngine.Events.PersistentCall"] = function (request, data, root) {
  var i2634 = root || request.c( 'UnityEngine.Events.PersistentCall' )
  var i2635 = data
  request.r(i2635[0], i2635[1], 0, i2634, 'm_Target')
  i2634.m_TargetAssemblyTypeName = i2635[2]
  i2634.m_MethodName = i2635[3]
  i2634.m_Mode = i2635[4]
  i2634.m_Arguments = request.d('UnityEngine.Events.ArgumentCache', i2635[5], i2634.m_Arguments)
  i2634.m_CallState = i2635[6]
  return i2634
}

Deserializers["UnityEngine.UI.Navigation"] = function (request, data, root) {
  var i2636 = root || request.c( 'UnityEngine.UI.Navigation' )
  var i2637 = data
  i2636.m_Mode = i2637[0]
  i2636.m_WrapAround = !!i2637[1]
  request.r(i2637[2], i2637[3], 0, i2636, 'm_SelectOnUp')
  request.r(i2637[4], i2637[5], 0, i2636, 'm_SelectOnDown')
  request.r(i2637[6], i2637[7], 0, i2636, 'm_SelectOnLeft')
  request.r(i2637[8], i2637[9], 0, i2636, 'm_SelectOnRight')
  return i2636
}

Deserializers["UnityEngine.UI.ColorBlock"] = function (request, data, root) {
  var i2638 = root || request.c( 'UnityEngine.UI.ColorBlock' )
  var i2639 = data
  i2638.m_NormalColor = new pc.Color(i2639[0], i2639[1], i2639[2], i2639[3])
  i2638.m_HighlightedColor = new pc.Color(i2639[4], i2639[5], i2639[6], i2639[7])
  i2638.m_PressedColor = new pc.Color(i2639[8], i2639[9], i2639[10], i2639[11])
  i2638.m_SelectedColor = new pc.Color(i2639[12], i2639[13], i2639[14], i2639[15])
  i2638.m_DisabledColor = new pc.Color(i2639[16], i2639[17], i2639[18], i2639[19])
  i2638.m_ColorMultiplier = i2639[20]
  i2638.m_FadeDuration = i2639[21]
  return i2638
}

Deserializers["UnityEngine.UI.SpriteState"] = function (request, data, root) {
  var i2640 = root || request.c( 'UnityEngine.UI.SpriteState' )
  var i2641 = data
  request.r(i2641[0], i2641[1], 0, i2640, 'm_HighlightedSprite')
  request.r(i2641[2], i2641[3], 0, i2640, 'm_PressedSprite')
  request.r(i2641[4], i2641[5], 0, i2640, 'm_SelectedSprite')
  request.r(i2641[6], i2641[7], 0, i2640, 'm_DisabledSprite')
  return i2640
}

Deserializers["UnityEngine.UI.AnimationTriggers"] = function (request, data, root) {
  var i2642 = root || request.c( 'UnityEngine.UI.AnimationTriggers' )
  var i2643 = data
  i2642.m_NormalTrigger = i2643[0]
  i2642.m_HighlightedTrigger = i2643[1]
  i2642.m_PressedTrigger = i2643[2]
  i2642.m_SelectedTrigger = i2643[3]
  i2642.m_DisabledTrigger = i2643[4]
  return i2642
}

Deserializers["SoundUIElement"] = function (request, data, root) {
  var i2644 = root || request.c( 'SoundUIElement' )
  var i2645 = data
  i2644.Sound = request.d('SoundDefine', i2645[0], i2644.Sound)
  i2644.PlayOnEnable = !!i2645[1]
  i2644.StopOnDisable = !!i2645[2]
  i2644.playWithInteractable = !!i2645[3]
  i2644.isPlayRandomBackGroundMusic = !!i2645[4]
  return i2644
}

Deserializers["SoundDefine"] = function (request, data, root) {
  var i2646 = root || request.c( 'SoundDefine' )
  var i2647 = data
  i2646.soundType = i2647[0]
  i2646.Loop = !!i2647[1]
  request.r(i2647[2], i2647[3], 0, i2646, 'Clip')
  var i2649 = i2647[4]
  var i2648 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.AudioClip')))
  for(var i = 0; i < i2649.length; i += 2) {
  request.r(i2649[i + 0], i2649[i + 1], 1, i2648, '')
  }
  i2646.ClipList = i2648
  return i2646
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh"] = function (request, data, root) {
  var i2652 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh' )
  var i2653 = data
  i2652.name = i2653[0]
  i2652.halfPrecision = !!i2653[1]
  i2652.useUInt32IndexFormat = !!i2653[2]
  i2652.vertexCount = i2653[3]
  i2652.aabb = i2653[4]
  var i2655 = i2653[5]
  var i2654 = []
  for(var i = 0; i < i2655.length; i += 1) {
    i2654.push( !!i2655[i + 0] );
  }
  i2652.streams = i2654
  i2652.vertices = i2653[6]
  var i2657 = i2653[7]
  var i2656 = []
  for(var i = 0; i < i2657.length; i += 1) {
    i2656.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh', i2657[i + 0]) );
  }
  i2652.subMeshes = i2656
  var i2659 = i2653[8]
  var i2658 = []
  for(var i = 0; i < i2659.length; i += 16) {
    i2658.push( new pc.Mat4().setData(i2659[i + 0], i2659[i + 1], i2659[i + 2], i2659[i + 3],  i2659[i + 4], i2659[i + 5], i2659[i + 6], i2659[i + 7],  i2659[i + 8], i2659[i + 9], i2659[i + 10], i2659[i + 11],  i2659[i + 12], i2659[i + 13], i2659[i + 14], i2659[i + 15]) );
  }
  i2652.bindposes = i2658
  var i2661 = i2653[9]
  var i2660 = []
  for(var i = 0; i < i2661.length; i += 1) {
    i2660.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape', i2661[i + 0]) );
  }
  i2652.blendShapes = i2660
  return i2652
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh"] = function (request, data, root) {
  var i2666 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh' )
  var i2667 = data
  i2666.triangles = i2667[0]
  return i2666
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape"] = function (request, data, root) {
  var i2672 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape' )
  var i2673 = data
  i2672.name = i2673[0]
  var i2675 = i2673[1]
  var i2674 = []
  for(var i = 0; i < i2675.length; i += 1) {
    i2674.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame', i2675[i + 0]) );
  }
  i2672.frames = i2674
  return i2672
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i2676 = root || new pc.UnityMaterial()
  var i2677 = data
  i2676.name = i2677[0]
  request.r(i2677[1], i2677[2], 0, i2676, 'shader')
  i2676.renderQueue = i2677[3]
  i2676.enableInstancing = !!i2677[4]
  var i2679 = i2677[5]
  var i2678 = []
  for(var i = 0; i < i2679.length; i += 1) {
    i2678.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i2679[i + 0]) );
  }
  i2676.floatParameters = i2678
  var i2681 = i2677[6]
  var i2680 = []
  for(var i = 0; i < i2681.length; i += 1) {
    i2680.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i2681[i + 0]) );
  }
  i2676.colorParameters = i2680
  var i2683 = i2677[7]
  var i2682 = []
  for(var i = 0; i < i2683.length; i += 1) {
    i2682.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i2683[i + 0]) );
  }
  i2676.vectorParameters = i2682
  var i2685 = i2677[8]
  var i2684 = []
  for(var i = 0; i < i2685.length; i += 1) {
    i2684.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i2685[i + 0]) );
  }
  i2676.textureParameters = i2684
  var i2687 = i2677[9]
  var i2686 = []
  for(var i = 0; i < i2687.length; i += 1) {
    i2686.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i2687[i + 0]) );
  }
  i2676.materialFlags = i2686
  return i2676
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i2690 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i2691 = data
  i2690.name = i2691[0]
  i2690.value = i2691[1]
  return i2690
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i2694 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i2695 = data
  i2694.name = i2695[0]
  i2694.value = new pc.Color(i2695[1], i2695[2], i2695[3], i2695[4])
  return i2694
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i2698 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i2699 = data
  i2698.name = i2699[0]
  i2698.value = new pc.Vec4( i2699[1], i2699[2], i2699[3], i2699[4] )
  return i2698
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i2702 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i2703 = data
  i2702.name = i2703[0]
  request.r(i2703[1], i2703[2], 0, i2702, 'value')
  return i2702
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i2706 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i2707 = data
  i2706.name = i2707[0]
  i2706.enabled = !!i2707[1]
  return i2706
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i2708 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i2709 = data
  i2708.position = new pc.Vec3( i2709[0], i2709[1], i2709[2] )
  i2708.scale = new pc.Vec3( i2709[3], i2709[4], i2709[5] )
  i2708.rotation = new pc.Quat(i2709[6], i2709[7], i2709[8], i2709[9])
  return i2708
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshFilter"] = function (request, data, root) {
  var i2710 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshFilter' )
  var i2711 = data
  request.r(i2711[0], i2711[1], 0, i2710, 'sharedMesh')
  return i2710
}

Deserializers["CubeTargetControl"] = function (request, data, root) {
  var i2712 = root || request.c( 'CubeTargetControl' )
  var i2713 = data
  var i2715 = i2713[0]
  var i2714 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Transform')))
  for(var i = 0; i < i2715.length; i += 2) {
  request.r(i2715[i + 0], i2715[i + 1], 1, i2714, '')
  }
  i2712.TargetChildren = i2714
  var i2717 = i2713[1]
  var i2716 = []
  for(var i = 0; i < i2717.length; i += 2) {
  request.r(i2717[i + 0], i2717[i + 1], 2, i2716, '')
  }
  i2712.MeshRenderer = i2716
  i2712.IsActive = !!i2713[2]
  i2712.RollWoolTime = i2713[3]
  i2712.DelayTime = i2713[4]
  i2712.VibrationStrength = i2713[5]
  request.r(i2713[6], i2713[7], 0, i2712, 'AddCubeIcon')
  request.r(i2713[8], i2713[9], 0, i2712, '_boxAnimation')
  i2712._boxMoveAnimation = i2713[10]
  request.r(i2713[11], i2713[12], 0, i2712, '_boxCollider')
  return i2712
}

Deserializers["TargetBoxAnimation"] = function (request, data, root) {
  var i2722 = root || request.c( 'TargetBoxAnimation' )
  var i2723 = data
  i2722._boxMoveTime = i2723[0]
  i2722._capMoveTime = i2723[1]
  i2722._hopDownTime = i2723[2]
  i2722._hopUpTime = i2723[3]
  request.r(i2723[4], i2723[5], 0, i2722, 'boxWhooshClip')
  request.r(i2723[6], i2723[7], 0, i2722, '_cap')
  request.r(i2723[8], i2723[9], 0, i2722, '_closeParticle')
  i2722._boxMoveDistance = i2723[10]
  i2722._capMoveDistance = i2723[11]
  i2722._capScaleTime = i2723[12]
  i2722._hopScale = i2723[13]
  i2722.offsetMoveY = i2723[14]
  return i2722
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.BoxCollider"] = function (request, data, root) {
  var i2724 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.BoxCollider' )
  var i2725 = data
  i2724.center = new pc.Vec3( i2725[0], i2725[1], i2725[2] )
  i2724.size = new pc.Vec3( i2725[3], i2725[4], i2725[5] )
  i2724.enabled = !!i2725[6]
  i2724.isTrigger = !!i2725[7]
  request.r(i2725[8], i2725[9], 0, i2724, 'material')
  return i2724
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshRenderer"] = function (request, data, root) {
  var i2726 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshRenderer' )
  var i2727 = data
  request.r(i2727[0], i2727[1], 0, i2726, 'additionalVertexStreams')
  i2726.enabled = !!i2727[2]
  request.r(i2727[3], i2727[4], 0, i2726, 'sharedMaterial')
  var i2729 = i2727[5]
  var i2728 = []
  for(var i = 0; i < i2729.length; i += 2) {
  request.r(i2729[i + 0], i2729[i + 1], 2, i2728, '')
  }
  i2726.sharedMaterials = i2728
  i2726.receiveShadows = !!i2727[6]
  i2726.shadowCastingMode = i2727[7]
  i2726.sortingLayerID = i2727[8]
  i2726.sortingOrder = i2727[9]
  i2726.lightmapIndex = i2727[10]
  i2726.lightmapSceneIndex = i2727[11]
  i2726.lightmapScaleOffset = new pc.Vec4( i2727[12], i2727[13], i2727[14], i2727[15] )
  i2726.lightProbeUsage = i2727[16]
  i2726.reflectionProbeUsage = i2727[17]
  return i2726
}

Deserializers["QueueTargetControl"] = function (request, data, root) {
  var i2732 = root || request.c( 'QueueTargetControl' )
  var i2733 = data
  return i2732
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystem"] = function (request, data, root) {
  var i2734 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystem' )
  var i2735 = data
  i2734.main = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule', i2735[0], i2734.main)
  i2734.colorBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule', i2735[1], i2734.colorBySpeed)
  i2734.colorOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule', i2735[2], i2734.colorOverLifetime)
  i2734.emission = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule', i2735[3], i2734.emission)
  i2734.rotationBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule', i2735[4], i2734.rotationBySpeed)
  i2734.rotationOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule', i2735[5], i2734.rotationOverLifetime)
  i2734.shape = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule', i2735[6], i2734.shape)
  i2734.sizeBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule', i2735[7], i2734.sizeBySpeed)
  i2734.sizeOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule', i2735[8], i2734.sizeOverLifetime)
  i2734.textureSheetAnimation = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule', i2735[9], i2734.textureSheetAnimation)
  i2734.velocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule', i2735[10], i2734.velocityOverLifetime)
  i2734.noise = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule', i2735[11], i2734.noise)
  i2734.inheritVelocity = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule', i2735[12], i2734.inheritVelocity)
  i2734.forceOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule', i2735[13], i2734.forceOverLifetime)
  i2734.limitVelocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule', i2735[14], i2734.limitVelocityOverLifetime)
  i2734.useAutoRandomSeed = !!i2735[15]
  i2734.randomSeed = i2735[16]
  return i2734
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule"] = function (request, data, root) {
  var i2736 = root || new pc.ParticleSystemMain()
  var i2737 = data
  i2736.duration = i2737[0]
  i2736.loop = !!i2737[1]
  i2736.prewarm = !!i2737[2]
  i2736.startDelay = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[3], i2736.startDelay)
  i2736.startLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[4], i2736.startLifetime)
  i2736.startSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[5], i2736.startSpeed)
  i2736.startSize3D = !!i2737[6]
  i2736.startSizeX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[7], i2736.startSizeX)
  i2736.startSizeY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[8], i2736.startSizeY)
  i2736.startSizeZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[9], i2736.startSizeZ)
  i2736.startRotation3D = !!i2737[10]
  i2736.startRotationX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[11], i2736.startRotationX)
  i2736.startRotationY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[12], i2736.startRotationY)
  i2736.startRotationZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[13], i2736.startRotationZ)
  i2736.startColor = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i2737[14], i2736.startColor)
  i2736.gravityModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2737[15], i2736.gravityModifier)
  i2736.simulationSpace = i2737[16]
  request.r(i2737[17], i2737[18], 0, i2736, 'customSimulationSpace')
  i2736.simulationSpeed = i2737[19]
  i2736.useUnscaledTime = !!i2737[20]
  i2736.scalingMode = i2737[21]
  i2736.playOnAwake = !!i2737[22]
  i2736.maxParticles = i2737[23]
  i2736.emitterVelocityMode = i2737[24]
  i2736.stopAction = i2737[25]
  return i2736
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve"] = function (request, data, root) {
  var i2738 = root || new pc.MinMaxCurve()
  var i2739 = data
  i2738.mode = i2739[0]
  i2738.curveMin = new pc.AnimationCurve( { keys_flow: i2739[1] } )
  i2738.curveMax = new pc.AnimationCurve( { keys_flow: i2739[2] } )
  i2738.curveMultiplier = i2739[3]
  i2738.constantMin = i2739[4]
  i2738.constantMax = i2739[5]
  return i2738
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient"] = function (request, data, root) {
  var i2740 = root || new pc.MinMaxGradient()
  var i2741 = data
  i2740.mode = i2741[0]
  i2740.gradientMin = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i2741[1], i2740.gradientMin)
  i2740.gradientMax = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i2741[2], i2740.gradientMax)
  i2740.colorMin = new pc.Color(i2741[3], i2741[4], i2741[5], i2741[6])
  i2740.colorMax = new pc.Color(i2741[7], i2741[8], i2741[9], i2741[10])
  return i2740
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient"] = function (request, data, root) {
  var i2742 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient' )
  var i2743 = data
  i2742.mode = i2743[0]
  var i2745 = i2743[1]
  var i2744 = []
  for(var i = 0; i < i2745.length; i += 1) {
    i2744.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey', i2745[i + 0]) );
  }
  i2742.colorKeys = i2744
  var i2747 = i2743[2]
  var i2746 = []
  for(var i = 0; i < i2747.length; i += 1) {
    i2746.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey', i2747[i + 0]) );
  }
  i2742.alphaKeys = i2746
  return i2742
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule"] = function (request, data, root) {
  var i2748 = root || new pc.ParticleSystemColorBySpeed()
  var i2749 = data
  i2748.enabled = !!i2749[0]
  i2748.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i2749[1], i2748.color)
  i2748.range = new pc.Vec2( i2749[2], i2749[3] )
  return i2748
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey"] = function (request, data, root) {
  var i2752 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey' )
  var i2753 = data
  i2752.color = new pc.Color(i2753[0], i2753[1], i2753[2], i2753[3])
  i2752.time = i2753[4]
  return i2752
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey"] = function (request, data, root) {
  var i2756 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey' )
  var i2757 = data
  i2756.alpha = i2757[0]
  i2756.time = i2757[1]
  return i2756
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule"] = function (request, data, root) {
  var i2758 = root || new pc.ParticleSystemColorOverLifetime()
  var i2759 = data
  i2758.enabled = !!i2759[0]
  i2758.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i2759[1], i2758.color)
  return i2758
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule"] = function (request, data, root) {
  var i2760 = root || new pc.ParticleSystemEmitter()
  var i2761 = data
  i2760.enabled = !!i2761[0]
  i2760.rateOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2761[1], i2760.rateOverTime)
  i2760.rateOverDistance = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2761[2], i2760.rateOverDistance)
  var i2763 = i2761[3]
  var i2762 = []
  for(var i = 0; i < i2763.length; i += 1) {
    i2762.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst', i2763[i + 0]) );
  }
  i2760.bursts = i2762
  return i2760
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst"] = function (request, data, root) {
  var i2766 = root || new pc.ParticleSystemBurst()
  var i2767 = data
  i2766.count = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2767[0], i2766.count)
  i2766.cycleCount = i2767[1]
  i2766.minCount = i2767[2]
  i2766.maxCount = i2767[3]
  i2766.repeatInterval = i2767[4]
  i2766.time = i2767[5]
  return i2766
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule"] = function (request, data, root) {
  var i2768 = root || new pc.ParticleSystemRotationBySpeed()
  var i2769 = data
  i2768.enabled = !!i2769[0]
  i2768.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2769[1], i2768.x)
  i2768.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2769[2], i2768.y)
  i2768.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2769[3], i2768.z)
  i2768.separateAxes = !!i2769[4]
  i2768.range = new pc.Vec2( i2769[5], i2769[6] )
  return i2768
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule"] = function (request, data, root) {
  var i2770 = root || new pc.ParticleSystemRotationOverLifetime()
  var i2771 = data
  i2770.enabled = !!i2771[0]
  i2770.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2771[1], i2770.x)
  i2770.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2771[2], i2770.y)
  i2770.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2771[3], i2770.z)
  i2770.separateAxes = !!i2771[4]
  return i2770
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule"] = function (request, data, root) {
  var i2772 = root || new pc.ParticleSystemShape()
  var i2773 = data
  i2772.enabled = !!i2773[0]
  i2772.shapeType = i2773[1]
  i2772.randomDirectionAmount = i2773[2]
  i2772.sphericalDirectionAmount = i2773[3]
  i2772.randomPositionAmount = i2773[4]
  i2772.alignToDirection = !!i2773[5]
  i2772.radius = i2773[6]
  i2772.radiusMode = i2773[7]
  i2772.radiusSpread = i2773[8]
  i2772.radiusSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2773[9], i2772.radiusSpeed)
  i2772.radiusThickness = i2773[10]
  i2772.angle = i2773[11]
  i2772.length = i2773[12]
  i2772.boxThickness = new pc.Vec3( i2773[13], i2773[14], i2773[15] )
  i2772.meshShapeType = i2773[16]
  request.r(i2773[17], i2773[18], 0, i2772, 'mesh')
  request.r(i2773[19], i2773[20], 0, i2772, 'meshRenderer')
  request.r(i2773[21], i2773[22], 0, i2772, 'skinnedMeshRenderer')
  i2772.useMeshMaterialIndex = !!i2773[23]
  i2772.meshMaterialIndex = i2773[24]
  i2772.useMeshColors = !!i2773[25]
  i2772.normalOffset = i2773[26]
  i2772.arc = i2773[27]
  i2772.arcMode = i2773[28]
  i2772.arcSpread = i2773[29]
  i2772.arcSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2773[30], i2772.arcSpeed)
  i2772.donutRadius = i2773[31]
  i2772.position = new pc.Vec3( i2773[32], i2773[33], i2773[34] )
  i2772.rotation = new pc.Vec3( i2773[35], i2773[36], i2773[37] )
  i2772.scale = new pc.Vec3( i2773[38], i2773[39], i2773[40] )
  return i2772
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule"] = function (request, data, root) {
  var i2774 = root || new pc.ParticleSystemSizeBySpeed()
  var i2775 = data
  i2774.enabled = !!i2775[0]
  i2774.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2775[1], i2774.x)
  i2774.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2775[2], i2774.y)
  i2774.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2775[3], i2774.z)
  i2774.separateAxes = !!i2775[4]
  i2774.range = new pc.Vec2( i2775[5], i2775[6] )
  return i2774
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule"] = function (request, data, root) {
  var i2776 = root || new pc.ParticleSystemSizeOverLifetime()
  var i2777 = data
  i2776.enabled = !!i2777[0]
  i2776.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2777[1], i2776.x)
  i2776.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2777[2], i2776.y)
  i2776.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2777[3], i2776.z)
  i2776.separateAxes = !!i2777[4]
  return i2776
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule"] = function (request, data, root) {
  var i2778 = root || new pc.ParticleSystemTextureSheetAnimation()
  var i2779 = data
  i2778.enabled = !!i2779[0]
  i2778.mode = i2779[1]
  i2778.animation = i2779[2]
  i2778.numTilesX = i2779[3]
  i2778.numTilesY = i2779[4]
  i2778.useRandomRow = !!i2779[5]
  i2778.frameOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2779[6], i2778.frameOverTime)
  i2778.startFrame = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2779[7], i2778.startFrame)
  i2778.cycleCount = i2779[8]
  i2778.rowIndex = i2779[9]
  i2778.flipU = i2779[10]
  i2778.flipV = i2779[11]
  i2778.spriteCount = i2779[12]
  var i2781 = i2779[13]
  var i2780 = []
  for(var i = 0; i < i2781.length; i += 2) {
  request.r(i2781[i + 0], i2781[i + 1], 2, i2780, '')
  }
  i2778.sprites = i2780
  return i2778
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule"] = function (request, data, root) {
  var i2784 = root || new pc.ParticleSystemVelocityOverLifetime()
  var i2785 = data
  i2784.enabled = !!i2785[0]
  i2784.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[1], i2784.x)
  i2784.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[2], i2784.y)
  i2784.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[3], i2784.z)
  i2784.radial = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[4], i2784.radial)
  i2784.speedModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[5], i2784.speedModifier)
  i2784.space = i2785[6]
  i2784.orbitalX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[7], i2784.orbitalX)
  i2784.orbitalY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[8], i2784.orbitalY)
  i2784.orbitalZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[9], i2784.orbitalZ)
  i2784.orbitalOffsetX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[10], i2784.orbitalOffsetX)
  i2784.orbitalOffsetY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[11], i2784.orbitalOffsetY)
  i2784.orbitalOffsetZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2785[12], i2784.orbitalOffsetZ)
  return i2784
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule"] = function (request, data, root) {
  var i2786 = root || new pc.ParticleSystemNoise()
  var i2787 = data
  i2786.enabled = !!i2787[0]
  i2786.separateAxes = !!i2787[1]
  i2786.strengthX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[2], i2786.strengthX)
  i2786.strengthY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[3], i2786.strengthY)
  i2786.strengthZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[4], i2786.strengthZ)
  i2786.frequency = i2787[5]
  i2786.damping = !!i2787[6]
  i2786.octaveCount = i2787[7]
  i2786.octaveMultiplier = i2787[8]
  i2786.octaveScale = i2787[9]
  i2786.quality = i2787[10]
  i2786.scrollSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[11], i2786.scrollSpeed)
  i2786.scrollSpeedMultiplier = i2787[12]
  i2786.remapEnabled = !!i2787[13]
  i2786.remapX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[14], i2786.remapX)
  i2786.remapY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[15], i2786.remapY)
  i2786.remapZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[16], i2786.remapZ)
  i2786.positionAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[17], i2786.positionAmount)
  i2786.rotationAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[18], i2786.rotationAmount)
  i2786.sizeAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2787[19], i2786.sizeAmount)
  return i2786
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule"] = function (request, data, root) {
  var i2788 = root || new pc.ParticleSystemInheritVelocity()
  var i2789 = data
  i2788.enabled = !!i2789[0]
  i2788.mode = i2789[1]
  i2788.curve = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2789[2], i2788.curve)
  return i2788
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule"] = function (request, data, root) {
  var i2790 = root || new pc.ParticleSystemForceOverLifetime()
  var i2791 = data
  i2790.enabled = !!i2791[0]
  i2790.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2791[1], i2790.x)
  i2790.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2791[2], i2790.y)
  i2790.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2791[3], i2790.z)
  i2790.space = i2791[4]
  i2790.randomized = !!i2791[5]
  return i2790
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule"] = function (request, data, root) {
  var i2792 = root || new pc.ParticleSystemLimitVelocityOverLifetime()
  var i2793 = data
  i2792.enabled = !!i2793[0]
  i2792.limit = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2793[1], i2792.limit)
  i2792.limitX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2793[2], i2792.limitX)
  i2792.limitY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2793[3], i2792.limitY)
  i2792.limitZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2793[4], i2792.limitZ)
  i2792.dampen = i2793[5]
  i2792.separateAxes = !!i2793[6]
  i2792.space = i2793[7]
  i2792.drag = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i2793[8], i2792.drag)
  i2792.multiplyDragByParticleSize = !!i2793[9]
  i2792.multiplyDragByParticleVelocity = !!i2793[10]
  return i2792
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer"] = function (request, data, root) {
  var i2794 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer' )
  var i2795 = data
  i2794.enabled = !!i2795[0]
  request.r(i2795[1], i2795[2], 0, i2794, 'sharedMaterial')
  var i2797 = i2795[3]
  var i2796 = []
  for(var i = 0; i < i2797.length; i += 2) {
  request.r(i2797[i + 0], i2797[i + 1], 2, i2796, '')
  }
  i2794.sharedMaterials = i2796
  i2794.receiveShadows = !!i2795[4]
  i2794.shadowCastingMode = i2795[5]
  i2794.sortingLayerID = i2795[6]
  i2794.sortingOrder = i2795[7]
  i2794.lightmapIndex = i2795[8]
  i2794.lightmapSceneIndex = i2795[9]
  i2794.lightmapScaleOffset = new pc.Vec4( i2795[10], i2795[11], i2795[12], i2795[13] )
  i2794.lightProbeUsage = i2795[14]
  i2794.reflectionProbeUsage = i2795[15]
  request.r(i2795[16], i2795[17], 0, i2794, 'mesh')
  i2794.meshCount = i2795[18]
  i2794.activeVertexStreamsCount = i2795[19]
  i2794.alignment = i2795[20]
  i2794.renderMode = i2795[21]
  i2794.sortMode = i2795[22]
  i2794.lengthScale = i2795[23]
  i2794.velocityScale = i2795[24]
  i2794.cameraVelocityScale = i2795[25]
  i2794.normalDirection = i2795[26]
  i2794.sortingFudge = i2795[27]
  i2794.minParticleSize = i2795[28]
  i2794.maxParticleSize = i2795[29]
  i2794.pivot = new pc.Vec3( i2795[30], i2795[31], i2795[32] )
  request.r(i2795[33], i2795[34], 0, i2794, 'trailMaterial')
  return i2794
}

Deserializers["HandController"] = function (request, data, root) {
  var i2798 = root || request.c( 'HandController' )
  var i2799 = data
  var i2801 = i2799[0]
  var i2800 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Sprite')))
  for(var i = 0; i < i2801.length; i += 2) {
  request.r(i2801[i + 0], i2801[i + 1], 1, i2800, '')
  }
  i2798.handSprites = i2800
  request.r(i2799[1], i2799[2], 0, i2798, 'handSpriteRenderer')
  i2798.positionShow = new pc.Vec3( i2799[3], i2799[4], i2799[5] )
  i2798.positionHide = new pc.Vec3( i2799[6], i2799[7], i2799[8] )
  i2798.delayTime = i2799[9]
  return i2798
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer"] = function (request, data, root) {
  var i2804 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer' )
  var i2805 = data
  i2804.enabled = !!i2805[0]
  request.r(i2805[1], i2805[2], 0, i2804, 'sharedMaterial')
  var i2807 = i2805[3]
  var i2806 = []
  for(var i = 0; i < i2807.length; i += 2) {
  request.r(i2807[i + 0], i2807[i + 1], 2, i2806, '')
  }
  i2804.sharedMaterials = i2806
  i2804.receiveShadows = !!i2805[4]
  i2804.shadowCastingMode = i2805[5]
  i2804.sortingLayerID = i2805[6]
  i2804.sortingOrder = i2805[7]
  i2804.lightmapIndex = i2805[8]
  i2804.lightmapSceneIndex = i2805[9]
  i2804.lightmapScaleOffset = new pc.Vec4( i2805[10], i2805[11], i2805[12], i2805[13] )
  i2804.lightProbeUsage = i2805[14]
  i2804.reflectionProbeUsage = i2805[15]
  i2804.color = new pc.Color(i2805[16], i2805[17], i2805[18], i2805[19])
  request.r(i2805[20], i2805[21], 0, i2804, 'sprite')
  i2804.flipX = !!i2805[22]
  i2804.flipY = !!i2805[23]
  i2804.drawMode = i2805[24]
  i2804.size = new pc.Vec2( i2805[25], i2805[26] )
  i2804.tileMode = i2805[27]
  i2804.adaptiveModeThreshold = i2805[28]
  i2804.maskInteraction = i2805[29]
  i2804.spriteSortPoint = i2805[30]
  return i2804
}

Deserializers["YarnWoolAnimation"] = function (request, data, root) {
  var i2808 = root || request.c( 'YarnWoolAnimation' )
  var i2809 = data
  request.r(i2809[0], i2809[1], 0, i2808, 'WoolAnimationData')
  request.r(i2809[2], i2809[3], 0, i2808, 'LineRenderer')
  return i2808
}

Deserializers["ObjectPool_Effect"] = function (request, data, root) {
  var i2810 = root || request.c( 'ObjectPool_Effect' )
  var i2811 = data
  i2810.waitTime = i2811[0]
  return i2810
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.LineRenderer"] = function (request, data, root) {
  var i2812 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.LineRenderer' )
  var i2813 = data
  i2812.textureMode = i2813[0]
  i2812.alignment = i2813[1]
  i2812.widthCurve = new pc.AnimationCurve( { keys_flow: i2813[2] } )
  i2812.colorGradient = i2813[3] ? new pc.ColorGradient(i2813[3][0], i2813[3][1], i2813[3][2]) : null
  var i2815 = i2813[4]
  var i2814 = []
  for(var i = 0; i < i2815.length; i += 3) {
    i2814.push( new pc.Vec3( i2815[i + 0], i2815[i + 1], i2815[i + 2] ) );
  }
  i2812.positions = i2814
  i2812.positionCount = i2813[5]
  i2812.widthMultiplier = i2813[6]
  i2812.startWidth = i2813[7]
  i2812.endWidth = i2813[8]
  i2812.numCornerVertices = i2813[9]
  i2812.numCapVertices = i2813[10]
  i2812.useWorldSpace = !!i2813[11]
  i2812.loop = !!i2813[12]
  i2812.startColor = new pc.Color(i2813[13], i2813[14], i2813[15], i2813[16])
  i2812.endColor = new pc.Color(i2813[17], i2813[18], i2813[19], i2813[20])
  i2812.generateLightingData = !!i2813[21]
  i2812.enabled = !!i2813[22]
  request.r(i2813[23], i2813[24], 0, i2812, 'sharedMaterial')
  var i2817 = i2813[25]
  var i2816 = []
  for(var i = 0; i < i2817.length; i += 2) {
  request.r(i2817[i + 0], i2817[i + 1], 2, i2816, '')
  }
  i2812.sharedMaterials = i2816
  i2812.receiveShadows = !!i2813[26]
  i2812.shadowCastingMode = i2813[27]
  i2812.sortingLayerID = i2813[28]
  i2812.sortingOrder = i2813[29]
  i2812.lightmapIndex = i2813[30]
  i2812.lightmapSceneIndex = i2813[31]
  i2812.lightmapScaleOffset = new pc.Vec4( i2813[32], i2813[33], i2813[34], i2813[35] )
  i2812.lightProbeUsage = i2813[36]
  i2812.reflectionProbeUsage = i2813[37]
  return i2812
}

Deserializers["RollWoolAnimation"] = function (request, data, root) {
  var i2820 = root || request.c( 'RollWoolAnimation' )
  var i2821 = data
  request.r(i2821[0], i2821[1], 0, i2820, 'woolClip1')
  request.r(i2821[2], i2821[3], 0, i2820, 'woolClip2')
  request.r(i2821[4], i2821[5], 0, i2820, 'WoolAnimationData')
  var i2823 = i2821[6]
  var i2822 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.MeshRenderer')))
  for(var i = 0; i < i2823.length; i += 2) {
  request.r(i2823[i + 0], i2823[i + 1], 1, i2822, '')
  }
  i2820.MeshRenderers = i2822
  i2820._localScale = new pc.Vec3( i2821[7], i2821[8], i2821[9] )
  return i2820
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Cubemap"] = function (request, data, root) {
  var i2826 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Cubemap' )
  var i2827 = data
  i2826.name = i2827[0]
  i2826.atlasId = i2827[1]
  i2826.mipmapCount = i2827[2]
  i2826.hdr = !!i2827[3]
  i2826.size = i2827[4]
  i2826.anisoLevel = i2827[5]
  i2826.filterMode = i2827[6]
  var i2829 = i2827[7]
  var i2828 = []
  for(var i = 0; i < i2829.length; i += 4) {
    i2828.push( UnityEngine.Rect.MinMaxRect(i2829[i + 0], i2829[i + 1], i2829[i + 2], i2829[i + 3]) );
  }
  i2826.rects = i2828
  i2826.wrapU = i2827[8]
  i2826.wrapV = i2827[9]
  return i2826
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i2832 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i2833 = data
  i2832.name = i2833[0]
  i2832.index = i2833[1]
  i2832.startup = !!i2833[2]
  return i2832
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Light"] = function (request, data, root) {
  var i2834 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Light' )
  var i2835 = data
  i2834.enabled = !!i2835[0]
  i2834.type = i2835[1]
  i2834.color = new pc.Color(i2835[2], i2835[3], i2835[4], i2835[5])
  i2834.cullingMask = i2835[6]
  i2834.intensity = i2835[7]
  i2834.range = i2835[8]
  i2834.spotAngle = i2835[9]
  i2834.shadows = i2835[10]
  i2834.shadowNormalBias = i2835[11]
  i2834.shadowBias = i2835[12]
  i2834.shadowStrength = i2835[13]
  i2834.shadowResolution = i2835[14]
  i2834.lightmapBakeType = i2835[15]
  i2834.renderMode = i2835[16]
  request.r(i2835[17], i2835[18], 0, i2834, 'cookie')
  i2834.cookieSize = i2835[19]
  return i2834
}

Deserializers["UnityEngine.UI.CanvasScaler"] = function (request, data, root) {
  var i2836 = root || request.c( 'UnityEngine.UI.CanvasScaler' )
  var i2837 = data
  i2836.m_UiScaleMode = i2837[0]
  i2836.m_ReferencePixelsPerUnit = i2837[1]
  i2836.m_ScaleFactor = i2837[2]
  i2836.m_ReferenceResolution = new pc.Vec2( i2837[3], i2837[4] )
  i2836.m_ScreenMatchMode = i2837[5]
  i2836.m_MatchWidthOrHeight = i2837[6]
  i2836.m_PhysicalUnit = i2837[7]
  i2836.m_FallbackScreenDPI = i2837[8]
  i2836.m_DefaultSpriteDPI = i2837[9]
  i2836.m_DynamicPixelsPerUnit = i2837[10]
  i2836.m_PresetInfoIsWorld = !!i2837[11]
  return i2836
}

Deserializers["Interactable"] = function (request, data, root) {
  var i2838 = root || request.c( 'Interactable' )
  var i2839 = data
  i2838.HoldThreshold = i2839[0]
  i2838.SwipeThreshold = i2839[1]
  return i2838
}

Deserializers["PlayNowButtonAnim"] = function (request, data, root) {
  var i2840 = root || request.c( 'PlayNowButtonAnim' )
  var i2841 = data
  request.r(i2841[0], i2841[1], 0, i2840, 'playerNowButton')
  i2840.maxScale = new pc.Vec3( i2841[2], i2841[3], i2841[4] )
  i2840.minScale = new pc.Vec3( i2841[5], i2841[6], i2841[7] )
  i2840.scaleDuration = i2841[8]
  return i2840
}

Deserializers["UnityEngine.UI.Outline"] = function (request, data, root) {
  var i2842 = root || request.c( 'UnityEngine.UI.Outline' )
  var i2843 = data
  i2842.m_EffectColor = new pc.Color(i2843[0], i2843[1], i2843[2], i2843[3])
  i2842.m_EffectDistance = new pc.Vec2( i2843[4], i2843[5] )
  i2842.m_UseGraphicAlpha = !!i2843[6]
  return i2842
}

Deserializers["UnityEngine.UI.Shadow"] = function (request, data, root) {
  var i2844 = root || request.c( 'UnityEngine.UI.Shadow' )
  var i2845 = data
  i2844.m_EffectColor = new pc.Color(i2845[0], i2845[1], i2845[2], i2845[3])
  i2844.m_EffectDistance = new pc.Vec2( i2845[4], i2845[5] )
  i2844.m_UseGraphicAlpha = !!i2845[6]
  return i2844
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.AudioSource"] = function (request, data, root) {
  var i2846 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.AudioSource' )
  var i2847 = data
  request.r(i2847[0], i2847[1], 0, i2846, 'clip')
  request.r(i2847[2], i2847[3], 0, i2846, 'outputAudioMixerGroup')
  i2846.playOnAwake = !!i2847[4]
  i2846.loop = !!i2847[5]
  i2846.time = i2847[6]
  i2846.volume = i2847[7]
  i2846.pitch = i2847[8]
  i2846.enabled = !!i2847[9]
  return i2846
}

Deserializers["CameraController"] = function (request, data, root) {
  var i2848 = root || request.c( 'CameraController' )
  var i2849 = data
  request.r(i2849[0], i2849[1], 0, i2848, 'InputInteractable')
  request.r(i2849[2], i2849[3], 0, i2848, 'ZoomCameraData')
  request.r(i2849[4], i2849[5], 0, i2848, 'BackGround')
  request.r(i2849[6], i2849[7], 0, i2848, 'SpawnPoint')
  request.r(i2849[8], i2849[9], 0, i2848, 'ModelPrefab')
  i2848.targetRotation = new pc.Quat(i2849[10], i2849[11], i2849[12], i2849[13])
  i2848.Friction = i2849[14]
  i2848.RotationSensitivity = new pc.Vec2( i2849[15], i2849[16] )
  i2848.AccelerationRange = new pc.Vec2( i2849[17], i2849[18] )
  i2848.RotationSpeed = i2849[19]
  i2848.RotationAutoSpeed = i2849[20]
  i2848.SmoothingTime = i2849[21]
  i2848.TimeAFKToAutoRotation = i2849[22]
  i2848.IntroLenght = i2849[23]
  i2848.ModelRotationIntroSpeed = i2849[24]
  i2848.IntroCameraZoomInDuration = i2849[25]
  i2848.IntroStartFOV = i2849[26]
  i2848.IntroEndFOV = i2849[27]
  i2848.DragStyle = i2849[28]
  i2848.DraggingSpeed = i2849[29]
  i2848.SmoothFactor = i2849[30]
  i2848.ZoomStyle = i2849[31]
  i2848._cameraPosMainMenuDefault = new pc.Vec3( i2849[32], i2849[33], i2849[34] )
  i2848._cameraRoteMainMenuDefault = new pc.Vec3( i2849[35], i2849[36], i2849[37] )
  i2848._cameraPosGamePlayDefault = new pc.Vec3( i2849[38], i2849[39], i2849[40] )
  i2848._cameraRoteGamePlayDefault = new pc.Vec3( i2849[41], i2849[42], i2849[43] )
  return i2848
}

Deserializers["GamePlaySystem"] = function (request, data, root) {
  var i2850 = root || request.c( 'GamePlaySystem' )
  var i2851 = data
  request.r(i2851[0], i2851[1], 0, i2850, 'CameraController')
  request.r(i2851[2], i2851[3], 0, i2850, 'BoxChainReaction3D')
  var i2853 = i2851[4]
  var i2852 = new (System.Collections.Generic.List$1(Bridge.ns('CubeTargetControl')))
  for(var i = 0; i < i2853.length; i += 2) {
  request.r(i2853[i + 0], i2853[i + 1], 1, i2852, '')
  }
  i2850.CurrentCubeTargets = i2852
  var i2855 = i2851[5]
  var i2854 = new (System.Collections.Generic.List$1(Bridge.ns('QueueTargetControl')))
  for(var i = 0; i < i2855.length; i += 2) {
  request.r(i2855[i + 0], i2855[i + 1], 1, i2854, '')
  }
  i2850.CurrentQueueTargets = i2854
  request.r(i2851[6], i2851[7], 0, i2850, 'YarnWoolPrefab')
  request.r(i2851[8], i2851[9], 0, i2850, 'RollWoolPrefab')
  i2850.TotalCubeActive = i2851[10]
  i2850.CubeReadyCount = i2851[11]
  request.r(i2851[12], i2851[13], 0, i2850, 'woolXoayClip')
  request.r(i2851[14], i2851[15], 0, i2850, 'wool1Clip')
  request.r(i2851[16], i2851[17], 0, i2850, 'loseSound')
  i2850.IsGoToStore = !!i2851[18]
  request.r(i2851[19], i2851[20], 0, i2850, 'handController')
  i2850.cubeCountClaimed = i2851[21]
  request.r(i2851[22], i2851[23], 0, i2850, 'endGamePanel')
  i2850.LoseOffer = i2851[24]
  i2850._cubeTargetCountDefault = i2851[25]
  request.r(i2851[26], i2851[27], 0, i2850, '_levelPrefab')
  i2850.spacingCubeTarget = i2851[28]
  return i2850
}

Deserializers["ObjectPool"] = function (request, data, root) {
  var i2860 = root || request.c( 'ObjectPool' )
  var i2861 = data
  return i2860
}

Deserializers["SoundManager"] = function (request, data, root) {
  var i2862 = root || request.c( 'SoundManager' )
  var i2863 = data
  request.r(i2863[0], i2863[1], 0, i2862, 'audioMixer')
  request.r(i2863[2], i2863[3], 0, i2862, 'fxMusicSource')
  request.r(i2863[4], i2863[5], 0, i2862, 'specialBgmSource')
  i2862.BGM = request.d('SoundDefine', i2863[6], i2862.BGM)
  return i2862
}

Deserializers["BoxChainReaction3D"] = function (request, data, root) {
  var i2864 = root || request.c( 'BoxChainReaction3D' )
  var i2865 = data
  i2864.InitialBoxCount = i2865[0]
  request.r(i2865[1], i2865[2], 0, i2864, 'boosterWhoosh1Clip')
  request.r(i2865[3], i2865[4], 0, i2864, 'boosterImpact1Clip')
  request.r(i2865[5], i2865[6], 0, i2864, 'boxContainer')
  request.r(i2865[7], i2865[8], 0, i2864, 'boxPrefab')
  i2864.initialSpacing = i2865[9]
  i2864.boxWidth = i2865[10]
  i2864.flyInDuration = i2865[11]
  i2864.collisionDuration = i2865[12]
  i2864.repositionDuration = i2865[13]
  i2864.collisionOffset = i2865[14]
  i2864.flyInStartPosition = new pc.Vec3( i2865[15], i2865[16], i2865[17] )
  i2864.flyInStretch = i2865[18]
  i2864.landingSquash = i2865[19]
  i2864.collisionSquash = i2865[20]
  i2864.collisionStretch = i2865[21]
  return i2864
}

Deserializers["GamePlayMeshController"] = function (request, data, root) {
  var i2866 = root || request.c( 'GamePlayMeshController' )
  var i2867 = data
  i2866.LevelId = i2867[0]
  i2866.IrgnoreLevelId = !!i2867[1]
  i2866.LevelData = request.d('LevelData', i2867[2], i2866.LevelData)
  var i2869 = i2867[3]
  var i2868 = new (System.Collections.Generic.List$1(Bridge.ns('WoolControl')))
  for(var i = 0; i < i2869.length; i += 2) {
  request.r(i2869[i + 0], i2869[i + 1], 1, i2868, '')
  }
  i2866.WoolControls = i2868
  request.r(i2867[4], i2867[5], 0, i2866, 'InterestCurveData')
  request.r(i2867[6], i2867[7], 0, i2866, 'WoolAnimationData')
  request.r(i2867[8], i2867[9], 0, i2866, 'WoolMaterial')
  request.r(i2867[10], i2867[11], 0, i2866, 'WoolChildMaterial')
  i2866.MaxLayerHasThreeSameColor = i2867[12]
  request.r(i2867[13], i2867[14], 0, i2866, 'CenterTransform')
  i2866._maxDistanceFromCetner = i2867[15]
  return i2866
}

Deserializers["LevelData"] = function (request, data, root) {
  var i2870 = root || request.c( 'LevelData' )
  var i2871 = data
  i2870.LevelId = i2871[0]
  i2870.CurrentcyLevel = i2871[1]
  i2870.DynamicDif = i2871[2]
  var i2873 = i2871[3]
  var i2872 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i2873.length; i += 4) {
    i2872.add(new pc.Color(i2873[i + 0], i2873[i + 1], i2873[i + 2], i2873[i + 3]));
  }
  i2870.ColorList = i2872
  var i2875 = i2871[4]
  var i2874 = new (System.Collections.Generic.List$1(Bridge.ns('System.Int32')))
  for(var i = 0; i < i2875.length; i += 1) {
    i2874.add(i2875[i + 0]);
  }
  i2870.ColorCountList = i2874
  return i2870
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshCollider"] = function (request, data, root) {
  var i2882 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshCollider' )
  var i2883 = data
  i2882.enabled = !!i2883[0]
  i2882.isTrigger = !!i2883[1]
  request.r(i2883[2], i2883[3], 0, i2882, 'material')
  request.r(i2883[4], i2883[5], 0, i2882, 'sharedMesh')
  i2882.convex = !!i2883[6]
  return i2882
}

Deserializers["WoolControl"] = function (request, data, root) {
  var i2884 = root || request.c( 'WoolControl' )
  var i2885 = data
  i2884.debugUV = !!i2885[0]
  i2884.MeshObjectData = request.d('MeshObjectData', i2885[1], i2884.MeshObjectData)
  request.r(i2885[2], i2885[3], 0, i2884, 'TopMeshRenderer')
  request.r(i2885[4], i2885[5], 0, i2884, 'HideMeshRenderer')
  request.r(i2885[6], i2885[7], 0, i2884, 'BoxCollider')
  request.r(i2885[8], i2885[9], 0, i2884, 'MainMaterial')
  request.r(i2885[10], i2885[11], 0, i2884, 'TranparentMaterial')
  request.r(i2885[12], i2885[13], 0, i2884, 'WoolAnimationData')
  var i2887 = i2885[14]
  var i2886 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i2887.length; i += 2) {
  request.r(i2887[i + 0], i2887[i + 1], 1, i2886, '')
  }
  i2884.DecoreControls = i2886
  var i2889 = i2885[15]
  var i2888 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i2889.length; i += 2) {
  request.r(i2889[i + 0], i2889[i + 1], 1, i2888, '')
  }
  i2884.RemovedDecoreControls = i2888
  request.r(i2885[16], i2885[17], 0, i2884, 'MeshFilter')
  i2884.IsSetColorHightest = !!i2885[18]
  var i2891 = i2885[19]
  var i2890 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Vector3')))
  for(var i = 0; i < i2891.length; i += 3) {
    i2890.add(new pc.Vec3( i2891[i + 0], i2891[i + 1], i2891[i + 2] ));
  }
  i2884._spiralPath = i2890
  var i2893 = i2885[20]
  var i2892 = new (System.Collections.Generic.List$1(Bridge.ns('System.Single')))
  for(var i = 0; i < i2893.length; i += 1) {
    i2892.add(i2893[i + 0]);
  }
  i2884._spiralPathUVY = i2892
  return i2884
}

Deserializers["MeshObjectData"] = function (request, data, root) {
  var i2894 = root || request.c( 'MeshObjectData' )
  var i2895 = data
  i2894.TotalLayer = i2895[0]
  i2894.HightestColor = new pc.Color(i2895[1], i2895[2], i2895[3], i2895[4])
  var i2897 = i2895[5]
  var i2896 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i2897.length; i += 4) {
    i2896.add(new pc.Color(i2897[i + 0], i2897[i + 1], i2897[i + 2], i2897[i + 3]));
  }
  i2894.ColorStack = i2896
  return i2894
}

Deserializers["DecoreControl"] = function (request, data, root) {
  var i2904 = root || request.c( 'DecoreControl' )
  var i2905 = data
  request.r(i2905[0], i2905[1], 0, i2904, 'Rigid')
  request.r(i2905[2], i2905[3], 0, i2904, 'MeshRenderer')
  i2904._color = new pc.Color(i2905[4], i2905[5], i2905[6], i2905[7])
  i2904.WoolProgressStartDrop = i2905[8]
  i2904._decoreState = i2905[9]
  return i2904
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Rigidbody"] = function (request, data, root) {
  var i2906 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Rigidbody' )
  var i2907 = data
  i2906.mass = i2907[0]
  i2906.drag = i2907[1]
  i2906.angularDrag = i2907[2]
  i2906.useGravity = !!i2907[3]
  i2906.isKinematic = !!i2907[4]
  i2906.constraints = i2907[5]
  i2906.maxAngularVelocity = i2907[6]
  i2906.collisionDetectionMode = i2907[7]
  i2906.interpolation = i2907[8]
  return i2906
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i2908 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i2909 = data
  i2908.enabled = !!i2909[0]
  i2908.aspect = i2909[1]
  i2908.orthographic = !!i2909[2]
  i2908.orthographicSize = i2909[3]
  i2908.backgroundColor = new pc.Color(i2909[4], i2909[5], i2909[6], i2909[7])
  i2908.nearClipPlane = i2909[8]
  i2908.farClipPlane = i2909[9]
  i2908.fieldOfView = i2909[10]
  i2908.depth = i2909[11]
  i2908.clearFlags = i2909[12]
  i2908.cullingMask = i2909[13]
  i2908.rect = i2909[14]
  request.r(i2909[15], i2909[16], 0, i2908, 'targetTexture')
  i2908.usePhysicalProperties = !!i2909[17]
  i2908.focalLength = i2909[18]
  i2908.sensorSize = new pc.Vec2( i2909[19], i2909[20] )
  i2908.lensShift = new pc.Vec2( i2909[21], i2909[22] )
  i2908.gateFit = i2909[23]
  i2908.commandBufferCount = i2909[24]
  i2908.cameraType = i2909[25]
  return i2908
}

Deserializers["CameraContainer"] = function (request, data, root) {
  var i2910 = root || request.c( 'CameraContainer' )
  var i2911 = data
  request.r(i2911[0], i2911[1], 0, i2910, 'MainCamera')
  request.r(i2911[2], i2911[3], 0, i2910, 'FakeUICamera')
  request.r(i2911[4], i2911[5], 0, i2910, 'EndgameModelCamera')
  return i2910
}

Deserializers["UnityEngine.EventSystems.EventSystem"] = function (request, data, root) {
  var i2912 = root || request.c( 'UnityEngine.EventSystems.EventSystem' )
  var i2913 = data
  request.r(i2913[0], i2913[1], 0, i2912, 'm_FirstSelected')
  i2912.m_sendNavigationEvents = !!i2913[2]
  i2912.m_DragThreshold = i2913[3]
  return i2912
}

Deserializers["UnityEngine.EventSystems.StandaloneInputModule"] = function (request, data, root) {
  var i2914 = root || request.c( 'UnityEngine.EventSystems.StandaloneInputModule' )
  var i2915 = data
  i2914.m_HorizontalAxis = i2915[0]
  i2914.m_VerticalAxis = i2915[1]
  i2914.m_SubmitButton = i2915[2]
  i2914.m_CancelButton = i2915[3]
  i2914.m_InputActionsPerSecond = i2915[4]
  i2914.m_RepeatDelay = i2915[5]
  i2914.m_ForceModuleActive = !!i2915[6]
  i2914.m_SendPointerHoverToParent = !!i2915[7]
  return i2914
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i2916 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i2917 = data
  i2916.ambientIntensity = i2917[0]
  i2916.reflectionIntensity = i2917[1]
  i2916.ambientMode = i2917[2]
  i2916.ambientLight = new pc.Color(i2917[3], i2917[4], i2917[5], i2917[6])
  i2916.ambientSkyColor = new pc.Color(i2917[7], i2917[8], i2917[9], i2917[10])
  i2916.ambientGroundColor = new pc.Color(i2917[11], i2917[12], i2917[13], i2917[14])
  i2916.ambientEquatorColor = new pc.Color(i2917[15], i2917[16], i2917[17], i2917[18])
  i2916.fogColor = new pc.Color(i2917[19], i2917[20], i2917[21], i2917[22])
  i2916.fogEndDistance = i2917[23]
  i2916.fogStartDistance = i2917[24]
  i2916.fogDensity = i2917[25]
  i2916.fog = !!i2917[26]
  request.r(i2917[27], i2917[28], 0, i2916, 'skybox')
  i2916.fogMode = i2917[29]
  var i2919 = i2917[30]
  var i2918 = []
  for(var i = 0; i < i2919.length; i += 1) {
    i2918.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i2919[i + 0]) );
  }
  i2916.lightmaps = i2918
  i2916.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i2917[31], i2916.lightProbes)
  i2916.lightmapsMode = i2917[32]
  i2916.mixedBakeMode = i2917[33]
  i2916.environmentLightingMode = i2917[34]
  i2916.ambientProbe = new pc.SphericalHarmonicsL2(i2917[35])
  i2916.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i2917[36])
  i2916.useReferenceAmbientProbe = !!i2917[37]
  request.r(i2917[38], i2917[39], 0, i2916, 'customReflection')
  request.r(i2917[40], i2917[41], 0, i2916, 'defaultReflection')
  i2916.defaultReflectionMode = i2917[42]
  i2916.defaultReflectionResolution = i2917[43]
  i2916.sunLightObjectId = i2917[44]
  i2916.pixelLightCount = i2917[45]
  i2916.defaultReflectionHDR = !!i2917[46]
  i2916.hasLightDataAsset = !!i2917[47]
  i2916.hasManualGenerate = !!i2917[48]
  return i2916
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i2922 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i2923 = data
  request.r(i2923[0], i2923[1], 0, i2922, 'lightmapColor')
  request.r(i2923[2], i2923[3], 0, i2922, 'lightmapDirection')
  return i2922
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i2924 = root || new UnityEngine.LightProbes()
  var i2925 = data
  return i2924
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i2930 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i2931 = data
  var i2933 = i2931[0]
  var i2932 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i2933.length; i += 1) {
    i2932.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i2933[i + 0]));
  }
  i2930.ShaderCompilationErrors = i2932
  i2930.name = i2931[1]
  i2930.guid = i2931[2]
  var i2935 = i2931[3]
  var i2934 = []
  for(var i = 0; i < i2935.length; i += 1) {
    i2934.push( i2935[i + 0] );
  }
  i2930.shaderDefinedKeywords = i2934
  var i2937 = i2931[4]
  var i2936 = []
  for(var i = 0; i < i2937.length; i += 1) {
    i2936.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i2937[i + 0]) );
  }
  i2930.passes = i2936
  var i2939 = i2931[5]
  var i2938 = []
  for(var i = 0; i < i2939.length; i += 1) {
    i2938.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i2939[i + 0]) );
  }
  i2930.usePasses = i2938
  var i2941 = i2931[6]
  var i2940 = []
  for(var i = 0; i < i2941.length; i += 1) {
    i2940.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i2941[i + 0]) );
  }
  i2930.defaultParameterValues = i2940
  request.r(i2931[7], i2931[8], 0, i2930, 'unityFallbackShader')
  i2930.readDepth = !!i2931[9]
  i2930.isCreatedByShaderGraph = !!i2931[10]
  i2930.compiled = !!i2931[11]
  return i2930
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i2944 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i2945 = data
  i2944.shaderName = i2945[0]
  i2944.errorMessage = i2945[1]
  return i2944
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i2950 = root || new pc.UnityShaderPass()
  var i2951 = data
  i2950.id = i2951[0]
  i2950.subShaderIndex = i2951[1]
  i2950.name = i2951[2]
  i2950.passType = i2951[3]
  i2950.grabPassTextureName = i2951[4]
  i2950.usePass = !!i2951[5]
  i2950.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[6], i2950.zTest)
  i2950.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[7], i2950.zWrite)
  i2950.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[8], i2950.culling)
  i2950.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i2951[9], i2950.blending)
  i2950.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i2951[10], i2950.alphaBlending)
  i2950.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[11], i2950.colorWriteMask)
  i2950.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[12], i2950.offsetUnits)
  i2950.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[13], i2950.offsetFactor)
  i2950.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[14], i2950.stencilRef)
  i2950.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[15], i2950.stencilReadMask)
  i2950.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2951[16], i2950.stencilWriteMask)
  i2950.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i2951[17], i2950.stencilOp)
  i2950.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i2951[18], i2950.stencilOpFront)
  i2950.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i2951[19], i2950.stencilOpBack)
  var i2953 = i2951[20]
  var i2952 = []
  for(var i = 0; i < i2953.length; i += 1) {
    i2952.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i2953[i + 0]) );
  }
  i2950.tags = i2952
  var i2955 = i2951[21]
  var i2954 = []
  for(var i = 0; i < i2955.length; i += 1) {
    i2954.push( i2955[i + 0] );
  }
  i2950.passDefinedKeywords = i2954
  var i2957 = i2951[22]
  var i2956 = []
  for(var i = 0; i < i2957.length; i += 1) {
    i2956.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i2957[i + 0]) );
  }
  i2950.passDefinedKeywordGroups = i2956
  var i2959 = i2951[23]
  var i2958 = []
  for(var i = 0; i < i2959.length; i += 1) {
    i2958.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i2959[i + 0]) );
  }
  i2950.variants = i2958
  var i2961 = i2951[24]
  var i2960 = []
  for(var i = 0; i < i2961.length; i += 1) {
    i2960.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i2961[i + 0]) );
  }
  i2950.excludedVariants = i2960
  i2950.hasDepthReader = !!i2951[25]
  return i2950
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i2962 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i2963 = data
  i2962.val = i2963[0]
  i2962.name = i2963[1]
  return i2962
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i2964 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i2965 = data
  i2964.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2965[0], i2964.src)
  i2964.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2965[1], i2964.dst)
  i2964.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2965[2], i2964.op)
  return i2964
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i2966 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i2967 = data
  i2966.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2967[0], i2966.pass)
  i2966.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2967[1], i2966.fail)
  i2966.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2967[2], i2966.zFail)
  i2966.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i2967[3], i2966.comp)
  return i2966
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i2970 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i2971 = data
  i2970.name = i2971[0]
  i2970.value = i2971[1]
  return i2970
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i2974 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i2975 = data
  var i2977 = i2975[0]
  var i2976 = []
  for(var i = 0; i < i2977.length; i += 1) {
    i2976.push( i2977[i + 0] );
  }
  i2974.keywords = i2976
  i2974.hasDiscard = !!i2975[1]
  return i2974
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i2980 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i2981 = data
  i2980.passId = i2981[0]
  i2980.subShaderIndex = i2981[1]
  var i2983 = i2981[2]
  var i2982 = []
  for(var i = 0; i < i2983.length; i += 1) {
    i2982.push( i2983[i + 0] );
  }
  i2980.keywords = i2982
  i2980.vertexProgram = i2981[3]
  i2980.fragmentProgram = i2981[4]
  i2980.exportedForWebGl2 = !!i2981[5]
  i2980.readDepth = !!i2981[6]
  return i2980
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i2986 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i2987 = data
  request.r(i2987[0], i2987[1], 0, i2986, 'shader')
  i2986.pass = i2987[2]
  return i2986
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i2990 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i2991 = data
  i2990.name = i2991[0]
  i2990.type = i2991[1]
  i2990.value = new pc.Vec4( i2991[2], i2991[3], i2991[4], i2991[5] )
  i2990.textureValue = i2991[6]
  i2990.shaderPropertyFlag = i2991[7]
  return i2990
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i2992 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i2993 = data
  i2992.name = i2993[0]
  request.r(i2993[1], i2993[2], 0, i2992, 'texture')
  i2992.aabb = i2993[3]
  i2992.vertices = i2993[4]
  i2992.triangles = i2993[5]
  i2992.textureRect = UnityEngine.Rect.MinMaxRect(i2993[6], i2993[7], i2993[8], i2993[9])
  i2992.packedRect = UnityEngine.Rect.MinMaxRect(i2993[10], i2993[11], i2993[12], i2993[13])
  i2992.border = new pc.Vec4( i2993[14], i2993[15], i2993[16], i2993[17] )
  i2992.transparency = i2993[18]
  i2992.bounds = i2993[19]
  i2992.pixelsPerUnit = i2993[20]
  i2992.textureWidth = i2993[21]
  i2992.textureHeight = i2993[22]
  i2992.nativeSize = new pc.Vec2( i2993[23], i2993[24] )
  i2992.pivot = new pc.Vec2( i2993[25], i2993[26] )
  i2992.textureRectOffset = new pc.Vec2( i2993[27], i2993[28] )
  return i2992
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.AudioClip"] = function (request, data, root) {
  var i2994 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.AudioClip' )
  var i2995 = data
  i2994.name = i2995[0]
  return i2994
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font"] = function (request, data, root) {
  var i2996 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font' )
  var i2997 = data
  i2996.name = i2997[0]
  i2996.ascent = i2997[1]
  i2996.originalLineHeight = i2997[2]
  i2996.fontSize = i2997[3]
  var i2999 = i2997[4]
  var i2998 = []
  for(var i = 0; i < i2999.length; i += 1) {
    i2998.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo', i2999[i + 0]) );
  }
  i2996.characterInfo = i2998
  request.r(i2997[5], i2997[6], 0, i2996, 'texture')
  i2996.originalFontSize = i2997[7]
  return i2996
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo"] = function (request, data, root) {
  var i3002 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo' )
  var i3003 = data
  i3002.index = i3003[0]
  i3002.advance = i3003[1]
  i3002.bearing = i3003[2]
  i3002.glyphWidth = i3003[3]
  i3002.glyphHeight = i3003[4]
  i3002.minX = i3003[5]
  i3002.maxX = i3003[6]
  i3002.minY = i3003[7]
  i3002.maxY = i3003[8]
  i3002.uvBottomLeftX = i3003[9]
  i3002.uvBottomLeftY = i3003[10]
  i3002.uvBottomRightX = i3003[11]
  i3002.uvBottomRightY = i3003[12]
  i3002.uvTopLeftX = i3003[13]
  i3002.uvTopLeftY = i3003[14]
  i3002.uvTopRightX = i3003[15]
  i3002.uvTopRightY = i3003[16]
  return i3002
}

Deserializers["WoolAnimationData"] = function (request, data, root) {
  var i3004 = root || request.c( 'WoolAnimationData' )
  var i3005 = data
  i3004.Duration = i3005[0]
  i3004.DurationHideWool = i3005[1]
  i3004.OffSet = i3005[2]
  i3004.ForceValue = i3005[3]
  i3004.RandomDirrectionFactor = i3005[4]
  return i3004
}

Deserializers["ZoomCameraData"] = function (request, data, root) {
  var i3006 = root || request.c( 'ZoomCameraData' )
  var i3007 = data
  i3006.ZoomSpeed = i3007[0]
  i3006.MinFOV = i3007[1]
  i3006.MaxFOV = i3007[2]
  i3006.DefaultFOV = i3007[3]
  return i3006
}

Deserializers["DG.Tweening.Core.DOTweenSettings"] = function (request, data, root) {
  var i3008 = root || request.c( 'DG.Tweening.Core.DOTweenSettings' )
  var i3009 = data
  i3008.useSafeMode = !!i3009[0]
  i3008.safeModeOptions = request.d('DG.Tweening.Core.DOTweenSettings+SafeModeOptions', i3009[1], i3008.safeModeOptions)
  i3008.timeScale = i3009[2]
  i3008.unscaledTimeScale = i3009[3]
  i3008.useSmoothDeltaTime = !!i3009[4]
  i3008.maxSmoothUnscaledTime = i3009[5]
  i3008.rewindCallbackMode = i3009[6]
  i3008.showUnityEditorReport = !!i3009[7]
  i3008.logBehaviour = i3009[8]
  i3008.drawGizmos = !!i3009[9]
  i3008.defaultRecyclable = !!i3009[10]
  i3008.defaultAutoPlay = i3009[11]
  i3008.defaultUpdateType = i3009[12]
  i3008.defaultTimeScaleIndependent = !!i3009[13]
  i3008.defaultEaseType = i3009[14]
  i3008.defaultEaseOvershootOrAmplitude = i3009[15]
  i3008.defaultEasePeriod = i3009[16]
  i3008.defaultAutoKill = !!i3009[17]
  i3008.defaultLoopType = i3009[18]
  i3008.debugMode = !!i3009[19]
  i3008.debugStoreTargetId = !!i3009[20]
  i3008.showPreviewPanel = !!i3009[21]
  i3008.storeSettingsLocation = i3009[22]
  i3008.modules = request.d('DG.Tweening.Core.DOTweenSettings+ModulesSetup', i3009[23], i3008.modules)
  i3008.createASMDEF = !!i3009[24]
  i3008.showPlayingTweens = !!i3009[25]
  i3008.showPausedTweens = !!i3009[26]
  return i3008
}

Deserializers["DG.Tweening.Core.DOTweenSettings+SafeModeOptions"] = function (request, data, root) {
  var i3010 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+SafeModeOptions' )
  var i3011 = data
  i3010.logBehaviour = i3011[0]
  i3010.nestedTweenFailureBehaviour = i3011[1]
  return i3010
}

Deserializers["DG.Tweening.Core.DOTweenSettings+ModulesSetup"] = function (request, data, root) {
  var i3012 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+ModulesSetup' )
  var i3013 = data
  i3012.showPanel = !!i3013[0]
  i3012.audioEnabled = !!i3013[1]
  i3012.physicsEnabled = !!i3013[2]
  i3012.physics2DEnabled = !!i3013[3]
  i3012.spriteEnabled = !!i3013[4]
  i3012.uiEnabled = !!i3013[5]
  i3012.textMeshProEnabled = !!i3013[6]
  i3012.tk2DEnabled = !!i3013[7]
  i3012.deAudioEnabled = !!i3013[8]
  i3012.deUnityExtendedEnabled = !!i3013[9]
  i3012.epoOutlineEnabled = !!i3013[10]
  return i3012
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i3014 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i3015 = data
  var i3017 = i3015[0]
  var i3016 = []
  for(var i = 0; i < i3017.length; i += 1) {
    i3016.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i3017[i + 0]) );
  }
  i3014.files = i3016
  i3014.componentToPrefabIds = i3015[1]
  return i3014
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i3020 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i3021 = data
  i3020.path = i3021[0]
  request.r(i3021[1], i3021[2], 0, i3020, 'unityObject')
  return i3020
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i3022 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i3023 = data
  var i3025 = i3023[0]
  var i3024 = []
  for(var i = 0; i < i3025.length; i += 1) {
    i3024.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i3025[i + 0]) );
  }
  i3022.scriptsExecutionOrder = i3024
  var i3027 = i3023[1]
  var i3026 = []
  for(var i = 0; i < i3027.length; i += 1) {
    i3026.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i3027[i + 0]) );
  }
  i3022.sortingLayers = i3026
  var i3029 = i3023[2]
  var i3028 = []
  for(var i = 0; i < i3029.length; i += 1) {
    i3028.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i3029[i + 0]) );
  }
  i3022.cullingLayers = i3028
  i3022.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i3023[3], i3022.timeSettings)
  i3022.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i3023[4], i3022.physicsSettings)
  i3022.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i3023[5], i3022.physics2DSettings)
  i3022.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i3023[6], i3022.qualitySettings)
  i3022.enableRealtimeShadows = !!i3023[7]
  i3022.enableAutoInstancing = !!i3023[8]
  i3022.enableDynamicBatching = !!i3023[9]
  i3022.lightmapEncodingQuality = i3023[10]
  i3022.desiredColorSpace = i3023[11]
  var i3031 = i3023[12]
  var i3030 = []
  for(var i = 0; i < i3031.length; i += 1) {
    i3030.push( i3031[i + 0] );
  }
  i3022.allTags = i3030
  return i3022
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i3034 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i3035 = data
  i3034.name = i3035[0]
  i3034.value = i3035[1]
  return i3034
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i3038 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i3039 = data
  i3038.id = i3039[0]
  i3038.name = i3039[1]
  i3038.value = i3039[2]
  return i3038
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i3042 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i3043 = data
  i3042.id = i3043[0]
  i3042.name = i3043[1]
  return i3042
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i3044 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i3045 = data
  i3044.fixedDeltaTime = i3045[0]
  i3044.maximumDeltaTime = i3045[1]
  i3044.timeScale = i3045[2]
  i3044.maximumParticleTimestep = i3045[3]
  return i3044
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i3046 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i3047 = data
  i3046.gravity = new pc.Vec3( i3047[0], i3047[1], i3047[2] )
  i3046.defaultSolverIterations = i3047[3]
  i3046.bounceThreshold = i3047[4]
  i3046.autoSyncTransforms = !!i3047[5]
  i3046.autoSimulation = !!i3047[6]
  var i3049 = i3047[7]
  var i3048 = []
  for(var i = 0; i < i3049.length; i += 1) {
    i3048.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i3049[i + 0]) );
  }
  i3046.collisionMatrix = i3048
  return i3046
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i3052 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i3053 = data
  i3052.enabled = !!i3053[0]
  i3052.layerId = i3053[1]
  i3052.otherLayerId = i3053[2]
  return i3052
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i3054 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i3055 = data
  request.r(i3055[0], i3055[1], 0, i3054, 'material')
  i3054.gravity = new pc.Vec2( i3055[2], i3055[3] )
  i3054.positionIterations = i3055[4]
  i3054.velocityIterations = i3055[5]
  i3054.velocityThreshold = i3055[6]
  i3054.maxLinearCorrection = i3055[7]
  i3054.maxAngularCorrection = i3055[8]
  i3054.maxTranslationSpeed = i3055[9]
  i3054.maxRotationSpeed = i3055[10]
  i3054.baumgarteScale = i3055[11]
  i3054.baumgarteTOIScale = i3055[12]
  i3054.timeToSleep = i3055[13]
  i3054.linearSleepTolerance = i3055[14]
  i3054.angularSleepTolerance = i3055[15]
  i3054.defaultContactOffset = i3055[16]
  i3054.autoSimulation = !!i3055[17]
  i3054.queriesHitTriggers = !!i3055[18]
  i3054.queriesStartInColliders = !!i3055[19]
  i3054.callbacksOnDisable = !!i3055[20]
  i3054.reuseCollisionCallbacks = !!i3055[21]
  i3054.autoSyncTransforms = !!i3055[22]
  var i3057 = i3055[23]
  var i3056 = []
  for(var i = 0; i < i3057.length; i += 1) {
    i3056.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i3057[i + 0]) );
  }
  i3054.collisionMatrix = i3056
  return i3054
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i3060 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i3061 = data
  i3060.enabled = !!i3061[0]
  i3060.layerId = i3061[1]
  i3060.otherLayerId = i3061[2]
  return i3060
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i3062 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i3063 = data
  var i3065 = i3063[0]
  var i3064 = []
  for(var i = 0; i < i3065.length; i += 1) {
    i3064.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i3065[i + 0]) );
  }
  i3062.qualityLevels = i3064
  var i3067 = i3063[1]
  var i3066 = []
  for(var i = 0; i < i3067.length; i += 1) {
    i3066.push( i3067[i + 0] );
  }
  i3062.names = i3066
  i3062.shadows = i3063[2]
  i3062.anisotropicFiltering = i3063[3]
  i3062.antiAliasing = i3063[4]
  i3062.lodBias = i3063[5]
  i3062.shadowCascades = i3063[6]
  i3062.shadowDistance = i3063[7]
  i3062.shadowmaskMode = i3063[8]
  i3062.shadowProjection = i3063[9]
  i3062.shadowResolution = i3063[10]
  i3062.softParticles = !!i3063[11]
  i3062.softVegetation = !!i3063[12]
  i3062.activeColorSpace = i3063[13]
  i3062.desiredColorSpace = i3063[14]
  i3062.masterTextureLimit = i3063[15]
  i3062.maxQueuedFrames = i3063[16]
  i3062.particleRaycastBudget = i3063[17]
  i3062.pixelLightCount = i3063[18]
  i3062.realtimeReflectionProbes = !!i3063[19]
  i3062.shadowCascade2Split = i3063[20]
  i3062.shadowCascade4Split = new pc.Vec3( i3063[21], i3063[22], i3063[23] )
  i3062.streamingMipmapsActive = !!i3063[24]
  i3062.vSyncCount = i3063[25]
  i3062.asyncUploadBufferSize = i3063[26]
  i3062.asyncUploadTimeSlice = i3063[27]
  i3062.billboardsFaceCameraPosition = !!i3063[28]
  i3062.shadowNearPlaneOffset = i3063[29]
  i3062.streamingMipmapsMemoryBudget = i3063[30]
  i3062.maximumLODLevel = i3063[31]
  i3062.streamingMipmapsAddAllCameras = !!i3063[32]
  i3062.streamingMipmapsMaxLevelReduction = i3063[33]
  i3062.streamingMipmapsRenderersPerFrame = i3063[34]
  i3062.resolutionScalingFixedDPIFactor = i3063[35]
  i3062.streamingMipmapsMaxFileIORequests = i3063[36]
  i3062.currentQualityLevel = i3063[37]
  return i3062
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixer"] = function (request, data, root) {
  var i3070 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixer' )
  var i3071 = data
  var i3073 = i3071[0]
  var i3072 = []
  for(var i = 0; i < i3073.length; i += 1) {
    i3072.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup', i3073[i + 0]) );
  }
  i3070.groups = i3072
  var i3075 = i3071[1]
  var i3074 = []
  for(var i = 0; i < i3075.length; i += 1) {
    i3074.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot', i3075[i + 0]) );
  }
  i3070.snapshots = i3074
  return i3070
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup"] = function (request, data, root) {
  var i3078 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup' )
  var i3079 = data
  i3078.id = i3079[0]
  i3078.childGroupIds = i3079[1]
  i3078.name = i3079[2]
  return i3078
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot"] = function (request, data, root) {
  var i3082 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot' )
  var i3083 = data
  i3082.id = i3083[0]
  var i3085 = i3083[1]
  var i3084 = []
  for(var i = 0; i < i3085.length; i += 1) {
    i3084.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter', i3085[i + 0]) );
  }
  i3082.parameters = i3084
  return i3082
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter"] = function (request, data, root) {
  var i3088 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter' )
  var i3089 = data
  i3088.name = i3089[0]
  i3088.value = i3089[1]
  return i3088
}

Deserializers["UnityEngine.Events.ArgumentCache"] = function (request, data, root) {
  var i3090 = root || request.c( 'UnityEngine.Events.ArgumentCache' )
  var i3091 = data
  request.r(i3091[0], i3091[1], 0, i3090, 'm_ObjectArgument')
  i3090.m_ObjectArgumentAssemblyTypeName = i3091[2]
  i3090.m_IntArgument = i3091[3]
  i3090.m_FloatArgument = i3091[4]
  i3090.m_StringArgument = i3091[5]
  i3090.m_BoolArgument = !!i3091[6]
  return i3090
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame"] = function (request, data, root) {
  var i3094 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame' )
  var i3095 = data
  i3094.weight = i3095[0]
  i3094.vertices = i3095[1]
  i3094.normals = i3095[2]
  i3094.tangents = i3095[3]
  return i3094
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Components.RectTransform":{"pivot":0,"anchorMin":2,"anchorMax":4,"sizeDelta":6,"anchoredPosition3D":8,"rotation":11,"scale":15},"Luna.Unity.DTO.UnityEngine.Components.CanvasGroup":{"m_Alpha":0,"m_Interactable":1,"m_BlocksRaycasts":2,"m_IgnoreParentGroups":3,"enabled":4},"Luna.Unity.DTO.UnityEngine.Components.Canvas":{"enabled":0,"planeDistance":1,"referencePixelsPerUnit":2,"isFallbackOverlay":3,"renderMode":4,"renderOrder":5,"sortingLayerName":6,"sortingOrder":7,"scaleFactor":8,"worldCamera":9,"overrideSorting":11,"pixelPerfect":12,"targetDisplay":13,"overridePixelPerfect":14},"Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer":{"cullTransparentMesh":0},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Assets.Mesh":{"name":0,"halfPrecision":1,"useUInt32IndexFormat":2,"vertexCount":3,"aabb":4,"streams":5,"vertices":6,"subMeshes":7,"bindposes":8,"blendShapes":9},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh":{"triangles":0},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape":{"name":0,"frames":1},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Components.MeshFilter":{"sharedMesh":0},"Luna.Unity.DTO.UnityEngine.Components.BoxCollider":{"center":0,"size":3,"enabled":6,"isTrigger":7,"material":8},"Luna.Unity.DTO.UnityEngine.Components.MeshRenderer":{"additionalVertexStreams":0,"enabled":2,"sharedMaterial":3,"sharedMaterials":5,"receiveShadows":6,"shadowCastingMode":7,"sortingLayerID":8,"sortingOrder":9,"lightmapIndex":10,"lightmapSceneIndex":11,"lightmapScaleOffset":12,"lightProbeUsage":16,"reflectionProbeUsage":17},"Luna.Unity.DTO.UnityEngine.Components.ParticleSystem":{"main":0,"colorBySpeed":1,"colorOverLifetime":2,"emission":3,"rotationBySpeed":4,"rotationOverLifetime":5,"shape":6,"sizeBySpeed":7,"sizeOverLifetime":8,"textureSheetAnimation":9,"velocityOverLifetime":10,"noise":11,"inheritVelocity":12,"forceOverLifetime":13,"limitVelocityOverLifetime":14,"useAutoRandomSeed":15,"randomSeed":16},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule":{"duration":0,"loop":1,"prewarm":2,"startDelay":3,"startLifetime":4,"startSpeed":5,"startSize3D":6,"startSizeX":7,"startSizeY":8,"startSizeZ":9,"startRotation3D":10,"startRotationX":11,"startRotationY":12,"startRotationZ":13,"startColor":14,"gravityModifier":15,"simulationSpace":16,"customSimulationSpace":17,"simulationSpeed":19,"useUnscaledTime":20,"scalingMode":21,"playOnAwake":22,"maxParticles":23,"emitterVelocityMode":24,"stopAction":25},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve":{"mode":0,"curveMin":1,"curveMax":2,"curveMultiplier":3,"constantMin":4,"constantMax":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient":{"mode":0,"gradientMin":1,"gradientMax":2,"colorMin":3,"colorMax":7},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient":{"mode":0,"colorKeys":1,"alphaKeys":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule":{"enabled":0,"color":1,"range":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey":{"color":0,"time":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey":{"alpha":0,"time":1},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule":{"enabled":0,"color":1},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule":{"enabled":0,"rateOverTime":1,"rateOverDistance":2,"bursts":3},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst":{"count":0,"cycleCount":1,"minCount":2,"maxCount":3,"repeatInterval":4,"time":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4,"range":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule":{"enabled":0,"shapeType":1,"randomDirectionAmount":2,"sphericalDirectionAmount":3,"randomPositionAmount":4,"alignToDirection":5,"radius":6,"radiusMode":7,"radiusSpread":8,"radiusSpeed":9,"radiusThickness":10,"angle":11,"length":12,"boxThickness":13,"meshShapeType":16,"mesh":17,"meshRenderer":19,"skinnedMeshRenderer":21,"useMeshMaterialIndex":23,"meshMaterialIndex":24,"useMeshColors":25,"normalOffset":26,"arc":27,"arcMode":28,"arcSpread":29,"arcSpeed":30,"donutRadius":31,"position":32,"rotation":35,"scale":38},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4,"range":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule":{"enabled":0,"mode":1,"animation":2,"numTilesX":3,"numTilesY":4,"useRandomRow":5,"frameOverTime":6,"startFrame":7,"cycleCount":8,"rowIndex":9,"flipU":10,"flipV":11,"spriteCount":12,"sprites":13},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"radial":4,"speedModifier":5,"space":6,"orbitalX":7,"orbitalY":8,"orbitalZ":9,"orbitalOffsetX":10,"orbitalOffsetY":11,"orbitalOffsetZ":12},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule":{"enabled":0,"separateAxes":1,"strengthX":2,"strengthY":3,"strengthZ":4,"frequency":5,"damping":6,"octaveCount":7,"octaveMultiplier":8,"octaveScale":9,"quality":10,"scrollSpeed":11,"scrollSpeedMultiplier":12,"remapEnabled":13,"remapX":14,"remapY":15,"remapZ":16,"positionAmount":17,"rotationAmount":18,"sizeAmount":19},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule":{"enabled":0,"mode":1,"curve":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"space":4,"randomized":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule":{"enabled":0,"limit":1,"limitX":2,"limitY":3,"limitZ":4,"dampen":5,"separateAxes":6,"space":7,"drag":8,"multiplyDragByParticleSize":9,"multiplyDragByParticleVelocity":10},"Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"mesh":16,"meshCount":18,"activeVertexStreamsCount":19,"alignment":20,"renderMode":21,"sortMode":22,"lengthScale":23,"velocityScale":24,"cameraVelocityScale":25,"normalDirection":26,"sortingFudge":27,"minParticleSize":28,"maxParticleSize":29,"pivot":30,"trailMaterial":33},"Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"color":16,"sprite":20,"flipX":22,"flipY":23,"drawMode":24,"size":25,"tileMode":27,"adaptiveModeThreshold":28,"maskInteraction":29,"spriteSortPoint":30},"Luna.Unity.DTO.UnityEngine.Components.LineRenderer":{"textureMode":0,"alignment":1,"widthCurve":2,"colorGradient":3,"positions":4,"positionCount":5,"widthMultiplier":6,"startWidth":7,"endWidth":8,"numCornerVertices":9,"numCapVertices":10,"useWorldSpace":11,"loop":12,"startColor":13,"endColor":17,"generateLightingData":21,"enabled":22,"sharedMaterial":23,"sharedMaterials":25,"receiveShadows":26,"shadowCastingMode":27,"sortingLayerID":28,"sortingOrder":29,"lightmapIndex":30,"lightmapSceneIndex":31,"lightmapScaleOffset":32,"lightProbeUsage":36,"reflectionProbeUsage":37},"Luna.Unity.DTO.UnityEngine.Textures.Cubemap":{"name":0,"atlasId":1,"mipmapCount":2,"hdr":3,"size":4,"anisoLevel":5,"filterMode":6,"rects":7,"wrapU":8,"wrapV":9},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Light":{"enabled":0,"type":1,"color":2,"cullingMask":6,"intensity":7,"range":8,"spotAngle":9,"shadows":10,"shadowNormalBias":11,"shadowBias":12,"shadowStrength":13,"shadowResolution":14,"lightmapBakeType":15,"renderMode":16,"cookie":17,"cookieSize":19},"Luna.Unity.DTO.UnityEngine.Components.AudioSource":{"clip":0,"outputAudioMixerGroup":2,"playOnAwake":4,"loop":5,"time":6,"volume":7,"pitch":8,"enabled":9},"Luna.Unity.DTO.UnityEngine.Components.MeshCollider":{"enabled":0,"isTrigger":1,"material":2,"sharedMesh":4,"convex":6},"Luna.Unity.DTO.UnityEngine.Components.Rigidbody":{"mass":0,"drag":1,"angularDrag":2,"useGravity":3,"isKinematic":4,"constraints":5,"maxAngularVelocity":6,"collisionDetectionMode":7,"interpolation":8},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"enabled":0,"aspect":1,"orthographic":2,"orthographicSize":3,"backgroundColor":4,"nearClipPlane":8,"farClipPlane":9,"fieldOfView":10,"depth":11,"clearFlags":12,"cullingMask":13,"rect":14,"targetTexture":15,"usePhysicalProperties":17,"focalLength":18,"sensorSize":19,"lensShift":21,"gateFit":23,"commandBufferCount":24,"cameraType":25},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"referenceAmbientProbe":36,"useReferenceAmbientProbe":37,"customReflection":38,"defaultReflection":40,"defaultReflectionMode":42,"defaultReflectionResolution":43,"sunLightObjectId":44,"pixelLightCount":45,"defaultReflectionHDR":46,"hasLightDataAsset":47,"hasManualGenerate":48},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"isCreatedByShaderGraph":10,"compiled":11},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Textures.Sprite":{"name":0,"texture":1,"aabb":3,"vertices":4,"triangles":5,"textureRect":6,"packedRect":10,"border":14,"transparency":18,"bounds":19,"pixelsPerUnit":20,"textureWidth":21,"textureHeight":22,"nativeSize":23,"pivot":25,"textureRectOffset":27},"Luna.Unity.DTO.UnityEngine.Assets.AudioClip":{"name":0},"Luna.Unity.DTO.UnityEngine.Assets.Font":{"name":0,"ascent":1,"originalLineHeight":2,"fontSize":3,"characterInfo":4,"texture":5,"originalFontSize":7},"Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo":{"index":0,"advance":1,"bearing":2,"glyphWidth":3,"glyphHeight":4,"minX":5,"maxX":6,"minY":7,"maxY":8,"uvBottomLeftX":9,"uvBottomLeftY":10,"uvBottomRightX":11,"uvBottomRightY":12,"uvTopLeftX":13,"uvTopLeftY":14,"uvTopRightX":15,"uvTopRightY":16},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableDynamicBatching":9,"lightmapEncodingQuality":10,"desiredColorSpace":11,"allTags":12},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixer":{"groups":0,"snapshots":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup":{"id":0,"childGroupIds":1,"name":2},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot":{"id":0,"parameters":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame":{"weight":0,"vertices":1,"normals":2,"tangents":3}}

Deserializers.requiredComponents = {"63":[64],"65":[64],"66":[64],"67":[64],"68":[64],"69":[64],"70":[71],"72":[36],"73":[56],"74":[56],"75":[56],"76":[56],"77":[56],"78":[56],"79":[56],"80":[81],"82":[81],"83":[81],"84":[81],"85":[81],"86":[81],"87":[81],"88":[81],"89":[81],"90":[81],"91":[81],"92":[81],"93":[81],"94":[36],"95":[19],"96":[97],"98":[97],"2":[0],"99":[100],"101":[100],"102":[0],"103":[0],"4":[2],"9":[8,0],"104":[0],"37":[2],"105":[0],"106":[0],"107":[0],"108":[0],"109":[0],"110":[0],"111":[0],"112":[0],"113":[0],"114":[8,0],"115":[0],"116":[0],"117":[0],"118":[0],"10":[8,0],"119":[0],"120":[59],"121":[59],"60":[59],"122":[59],"123":[36],"124":[36],"125":[100]}

Deserializers.types = ["UnityEngine.RectTransform","UnityEngine.CanvasGroup","UnityEngine.Canvas","UnityEngine.EventSystems.UIBehaviour","UnityEngine.UI.GraphicRaycaster","UnityEngine.MonoBehaviour","EndGameUI","UnityEngine.UI.Button","UnityEngine.CanvasRenderer","UnityEngine.UI.Image","UnityEngine.UI.Text","UnityEngine.Font","SoundUIElement","UnityEngine.AudioClip","UnityEngine.Shader","UnityEngine.Transform","UnityEngine.MeshFilter","UnityEngine.Mesh","CubeTargetControl","UnityEngine.MeshRenderer","TargetBoxAnimation","UnityEngine.BoxCollider","UnityEngine.ParticleSystem","UnityEngine.Material","QueueTargetControl","UnityEngine.ParticleSystemRenderer","UnityEngine.Texture2D","HandController","UnityEngine.Sprite","UnityEngine.SpriteRenderer","YarnWoolAnimation","WoolAnimationData","UnityEngine.LineRenderer","ObjectPool_Effect","RollWoolAnimation","UnityEngine.Light","UnityEngine.Camera","UnityEngine.UI.CanvasScaler","Interactable","PlayNowButtonAnim","UnityEngine.UI.Outline","UnityEngine.UI.Shadow","UnityEngine.AudioSource","CameraController","ZoomCameraData","UnityEngine.GameObject","GamePlaySystem","BoxChainReaction3D","ObjectPool","SoundManager","UnityEditor.Audio.AudioMixerController","UnityEditor.Audio.AudioMixerGroupController","GamePlayMeshController","WoolControl","UnityEngine.MeshCollider","DecoreControl","UnityEngine.Rigidbody","UnityEngine.AudioListener","CameraContainer","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.Cubemap","DG.Tweening.Core.DOTweenSettings","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.SkinnedMeshRenderer","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","Unity.VisualScripting.StateMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","Unity.VisualScripting.ScriptMachine"]

Deserializers.unityVersion = "2022.3.62f1";

Deserializers.productName = "WD_PlayableAds";

Deserializers.lunaInitializationTime = "06/06/2025 12:14:15";

Deserializers.lunaDaysRunning = "4.6";

Deserializers.lunaVersion = "6.3.0";

Deserializers.lunaSHA = "7c1090235e749b60367a931fd9d8e53ca14842b9";

Deserializers.creativeName = "WoolDom_3D";

Deserializers.lunaAppID = "30424";

Deserializers.projectId = "1d50191528698274c8378a20ad162070";

Deserializers.packagesInfo = "com.unity.ugui: 1.0.0";

Deserializers.externalJsLibraries = "";

Deserializers.androidLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.androidLink?window.$environment.packageConfig.androidLink:'Empty';

Deserializers.iosLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.iosLink?window.$environment.packageConfig.iosLink:'Empty';

Deserializers.base64Enabled = "False";

Deserializers.minifyEnabled = "True";

Deserializers.isForceUncompressed = "False";

Deserializers.isAntiAliasingEnabled = "False";

Deserializers.isRuntimeAnalysisEnabledForCode = "True";

Deserializers.runtimeAnalysisExcludedClassesCount = "1644";

Deserializers.runtimeAnalysisExcludedMethodsCount = "3785";

Deserializers.runtimeAnalysisExcludedModules = "physics2d, mecanim-wasm";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isReferenceAmbientProbeBaked = "False";

Deserializers.isLunaCompilerV2Used = "True";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "StandaloneWindows64";

Deserializers.applicationIdentifier = "com.DefaultCompany.WD-PlayableAds";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 28;

Deserializers.linearColorSpace = false;

Deserializers.buildID = "80b7062d-0f38-40a9-944e-ba840f1f5e71";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

