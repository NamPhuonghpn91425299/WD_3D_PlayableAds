var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i3612 = root || request.c( 'UnityEngine.JointSpring' )
  var i3613 = data
  i3612.spring = i3613[0]
  i3612.damper = i3613[1]
  i3612.targetPosition = i3613[2]
  return i3612
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i3614 = root || request.c( 'UnityEngine.JointMotor' )
  var i3615 = data
  i3614.m_TargetVelocity = i3615[0]
  i3614.m_Force = i3615[1]
  i3614.m_FreeSpin = i3615[2]
  return i3614
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i3616 = root || request.c( 'UnityEngine.JointLimits' )
  var i3617 = data
  i3616.m_Min = i3617[0]
  i3616.m_Max = i3617[1]
  i3616.m_Bounciness = i3617[2]
  i3616.m_BounceMinVelocity = i3617[3]
  i3616.m_ContactDistance = i3617[4]
  i3616.minBounce = i3617[5]
  i3616.maxBounce = i3617[6]
  return i3616
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i3618 = root || request.c( 'UnityEngine.JointDrive' )
  var i3619 = data
  i3618.m_PositionSpring = i3619[0]
  i3618.m_PositionDamper = i3619[1]
  i3618.m_MaximumForce = i3619[2]
  i3618.m_UseAcceleration = i3619[3]
  return i3618
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i3620 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i3621 = data
  i3620.m_Spring = i3621[0]
  i3620.m_Damper = i3621[1]
  return i3620
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i3622 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i3623 = data
  i3622.m_Limit = i3623[0]
  i3622.m_Bounciness = i3623[1]
  i3622.m_ContactDistance = i3623[2]
  return i3622
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i3624 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i3625 = data
  i3624.m_ExtremumSlip = i3625[0]
  i3624.m_ExtremumValue = i3625[1]
  i3624.m_AsymptoteSlip = i3625[2]
  i3624.m_AsymptoteValue = i3625[3]
  i3624.m_Stiffness = i3625[4]
  return i3624
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i3626 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i3627 = data
  i3626.m_LowerAngle = i3627[0]
  i3626.m_UpperAngle = i3627[1]
  return i3626
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i3628 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i3629 = data
  i3628.m_MotorSpeed = i3629[0]
  i3628.m_MaximumMotorTorque = i3629[1]
  return i3628
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i3630 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i3631 = data
  i3630.m_DampingRatio = i3631[0]
  i3630.m_Frequency = i3631[1]
  i3630.m_Angle = i3631[2]
  return i3630
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i3632 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i3633 = data
  i3632.m_LowerTranslation = i3633[0]
  i3632.m_UpperTranslation = i3633[1]
  return i3632
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i3634 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i3635 = data
  i3634.name = i3635[0]
  i3634.width = i3635[1]
  i3634.height = i3635[2]
  i3634.mipmapCount = i3635[3]
  i3634.anisoLevel = i3635[4]
  i3634.filterMode = i3635[5]
  i3634.hdr = !!i3635[6]
  i3634.format = i3635[7]
  i3634.wrapMode = i3635[8]
  i3634.alphaIsTransparency = !!i3635[9]
  i3634.alphaSource = i3635[10]
  i3634.graphicsFormat = i3635[11]
  i3634.sRGBTexture = !!i3635[12]
  i3634.desiredColorSpace = i3635[13]
  i3634.wrapU = i3635[14]
  i3634.wrapV = i3635[15]
  return i3634
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.RectTransform"] = function (request, data, root) {
  var i3636 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.RectTransform' )
  var i3637 = data
  i3636.pivot = new pc.Vec2( i3637[0], i3637[1] )
  i3636.anchorMin = new pc.Vec2( i3637[2], i3637[3] )
  i3636.anchorMax = new pc.Vec2( i3637[4], i3637[5] )
  i3636.sizeDelta = new pc.Vec2( i3637[6], i3637[7] )
  i3636.anchoredPosition3D = new pc.Vec3( i3637[8], i3637[9], i3637[10] )
  i3636.rotation = new pc.Quat(i3637[11], i3637[12], i3637[13], i3637[14])
  i3636.scale = new pc.Vec3( i3637[15], i3637[16], i3637[17] )
  return i3636
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasGroup"] = function (request, data, root) {
  var i3638 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasGroup' )
  var i3639 = data
  i3638.m_Alpha = i3639[0]
  i3638.m_Interactable = !!i3639[1]
  i3638.m_BlocksRaycasts = !!i3639[2]
  i3638.m_IgnoreParentGroups = !!i3639[3]
  i3638.enabled = !!i3639[4]
  return i3638
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Canvas"] = function (request, data, root) {
  var i3640 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Canvas' )
  var i3641 = data
  i3640.enabled = !!i3641[0]
  i3640.planeDistance = i3641[1]
  i3640.referencePixelsPerUnit = i3641[2]
  i3640.isFallbackOverlay = !!i3641[3]
  i3640.renderMode = i3641[4]
  i3640.renderOrder = i3641[5]
  i3640.sortingLayerName = i3641[6]
  i3640.sortingOrder = i3641[7]
  i3640.scaleFactor = i3641[8]
  request.r(i3641[9], i3641[10], 0, i3640, 'worldCamera')
  i3640.overrideSorting = !!i3641[11]
  i3640.pixelPerfect = !!i3641[12]
  i3640.targetDisplay = i3641[13]
  i3640.overridePixelPerfect = !!i3641[14]
  return i3640
}

Deserializers["UnityEngine.UI.GraphicRaycaster"] = function (request, data, root) {
  var i3642 = root || request.c( 'UnityEngine.UI.GraphicRaycaster' )
  var i3643 = data
  i3642.m_IgnoreReversedGraphics = !!i3643[0]
  i3642.m_BlockingObjects = i3643[1]
  i3642.m_BlockingMask = UnityEngine.LayerMask.FromIntegerValue( i3643[2] )
  return i3642
}

Deserializers["EndGameUI"] = function (request, data, root) {
  var i3644 = root || request.c( 'EndGameUI' )
  var i3645 = data
  request.r(i3645[0], i3645[1], 0, i3644, 'replayButton')
  i3644.maxScale = new pc.Vec3( i3645[2], i3645[3], i3645[4] )
  i3644.minScale = new pc.Vec3( i3645[5], i3645[6], i3645[7] )
  i3644.scaleDuration = i3645[8]
  return i3644
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer"] = function (request, data, root) {
  var i3646 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer' )
  var i3647 = data
  i3646.cullTransparentMesh = !!i3647[0]
  return i3646
}

Deserializers["UnityEngine.UI.Image"] = function (request, data, root) {
  var i3648 = root || request.c( 'UnityEngine.UI.Image' )
  var i3649 = data
  request.r(i3649[0], i3649[1], 0, i3648, 'm_Sprite')
  i3648.m_Type = i3649[2]
  i3648.m_PreserveAspect = !!i3649[3]
  i3648.m_FillCenter = !!i3649[4]
  i3648.m_FillMethod = i3649[5]
  i3648.m_FillAmount = i3649[6]
  i3648.m_FillClockwise = !!i3649[7]
  i3648.m_FillOrigin = i3649[8]
  i3648.m_UseSpriteMesh = !!i3649[9]
  i3648.m_PixelsPerUnitMultiplier = i3649[10]
  request.r(i3649[11], i3649[12], 0, i3648, 'm_Material')
  i3648.m_Maskable = !!i3649[13]
  i3648.m_Color = new pc.Color(i3649[14], i3649[15], i3649[16], i3649[17])
  i3648.m_RaycastTarget = !!i3649[18]
  i3648.m_RaycastPadding = new pc.Vec4( i3649[19], i3649[20], i3649[21], i3649[22] )
  return i3648
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i3650 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i3651 = data
  i3650.name = i3651[0]
  i3650.tagId = i3651[1]
  i3650.enabled = !!i3651[2]
  i3650.isStatic = !!i3651[3]
  i3650.layer = i3651[4]
  return i3650
}

Deserializers["UnityEngine.UI.Text"] = function (request, data, root) {
  var i3652 = root || request.c( 'UnityEngine.UI.Text' )
  var i3653 = data
  i3652.m_FontData = request.d('UnityEngine.UI.FontData', i3653[0], i3652.m_FontData)
  i3652.m_Text = i3653[1]
  request.r(i3653[2], i3653[3], 0, i3652, 'm_Material')
  i3652.m_Maskable = !!i3653[4]
  i3652.m_Color = new pc.Color(i3653[5], i3653[6], i3653[7], i3653[8])
  i3652.m_RaycastTarget = !!i3653[9]
  i3652.m_RaycastPadding = new pc.Vec4( i3653[10], i3653[11], i3653[12], i3653[13] )
  return i3652
}

Deserializers["UnityEngine.UI.FontData"] = function (request, data, root) {
  var i3654 = root || request.c( 'UnityEngine.UI.FontData' )
  var i3655 = data
  request.r(i3655[0], i3655[1], 0, i3654, 'm_Font')
  i3654.m_FontSize = i3655[2]
  i3654.m_FontStyle = i3655[3]
  i3654.m_BestFit = !!i3655[4]
  i3654.m_MinSize = i3655[5]
  i3654.m_MaxSize = i3655[6]
  i3654.m_Alignment = i3655[7]
  i3654.m_AlignByGeometry = !!i3655[8]
  i3654.m_RichText = !!i3655[9]
  i3654.m_HorizontalOverflow = i3655[10]
  i3654.m_VerticalOverflow = i3655[11]
  i3654.m_LineSpacing = i3655[12]
  return i3654
}

Deserializers["UnityEngine.UI.Button"] = function (request, data, root) {
  var i3656 = root || request.c( 'UnityEngine.UI.Button' )
  var i3657 = data
  i3656.m_OnClick = request.d('UnityEngine.UI.Button+ButtonClickedEvent', i3657[0], i3656.m_OnClick)
  i3656.m_Navigation = request.d('UnityEngine.UI.Navigation', i3657[1], i3656.m_Navigation)
  i3656.m_Transition = i3657[2]
  i3656.m_Colors = request.d('UnityEngine.UI.ColorBlock', i3657[3], i3656.m_Colors)
  i3656.m_SpriteState = request.d('UnityEngine.UI.SpriteState', i3657[4], i3656.m_SpriteState)
  i3656.m_AnimationTriggers = request.d('UnityEngine.UI.AnimationTriggers', i3657[5], i3656.m_AnimationTriggers)
  i3656.m_Interactable = !!i3657[6]
  request.r(i3657[7], i3657[8], 0, i3656, 'm_TargetGraphic')
  return i3656
}

Deserializers["UnityEngine.UI.Button+ButtonClickedEvent"] = function (request, data, root) {
  var i3658 = root || request.c( 'UnityEngine.UI.Button+ButtonClickedEvent' )
  var i3659 = data
  i3658.m_PersistentCalls = request.d('UnityEngine.Events.PersistentCallGroup', i3659[0], i3658.m_PersistentCalls)
  return i3658
}

Deserializers["UnityEngine.Events.PersistentCallGroup"] = function (request, data, root) {
  var i3660 = root || request.c( 'UnityEngine.Events.PersistentCallGroup' )
  var i3661 = data
  var i3663 = i3661[0]
  var i3662 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Events.PersistentCall')))
  for(var i = 0; i < i3663.length; i += 1) {
    i3662.add(request.d('UnityEngine.Events.PersistentCall', i3663[i + 0]));
  }
  i3660.m_Calls = i3662
  return i3660
}

Deserializers["UnityEngine.Events.PersistentCall"] = function (request, data, root) {
  var i3666 = root || request.c( 'UnityEngine.Events.PersistentCall' )
  var i3667 = data
  request.r(i3667[0], i3667[1], 0, i3666, 'm_Target')
  i3666.m_TargetAssemblyTypeName = i3667[2]
  i3666.m_MethodName = i3667[3]
  i3666.m_Mode = i3667[4]
  i3666.m_Arguments = request.d('UnityEngine.Events.ArgumentCache', i3667[5], i3666.m_Arguments)
  i3666.m_CallState = i3667[6]
  return i3666
}

Deserializers["UnityEngine.UI.Navigation"] = function (request, data, root) {
  var i3668 = root || request.c( 'UnityEngine.UI.Navigation' )
  var i3669 = data
  i3668.m_Mode = i3669[0]
  i3668.m_WrapAround = !!i3669[1]
  request.r(i3669[2], i3669[3], 0, i3668, 'm_SelectOnUp')
  request.r(i3669[4], i3669[5], 0, i3668, 'm_SelectOnDown')
  request.r(i3669[6], i3669[7], 0, i3668, 'm_SelectOnLeft')
  request.r(i3669[8], i3669[9], 0, i3668, 'm_SelectOnRight')
  return i3668
}

Deserializers["UnityEngine.UI.ColorBlock"] = function (request, data, root) {
  var i3670 = root || request.c( 'UnityEngine.UI.ColorBlock' )
  var i3671 = data
  i3670.m_NormalColor = new pc.Color(i3671[0], i3671[1], i3671[2], i3671[3])
  i3670.m_HighlightedColor = new pc.Color(i3671[4], i3671[5], i3671[6], i3671[7])
  i3670.m_PressedColor = new pc.Color(i3671[8], i3671[9], i3671[10], i3671[11])
  i3670.m_SelectedColor = new pc.Color(i3671[12], i3671[13], i3671[14], i3671[15])
  i3670.m_DisabledColor = new pc.Color(i3671[16], i3671[17], i3671[18], i3671[19])
  i3670.m_ColorMultiplier = i3671[20]
  i3670.m_FadeDuration = i3671[21]
  return i3670
}

Deserializers["UnityEngine.UI.SpriteState"] = function (request, data, root) {
  var i3672 = root || request.c( 'UnityEngine.UI.SpriteState' )
  var i3673 = data
  request.r(i3673[0], i3673[1], 0, i3672, 'm_HighlightedSprite')
  request.r(i3673[2], i3673[3], 0, i3672, 'm_PressedSprite')
  request.r(i3673[4], i3673[5], 0, i3672, 'm_SelectedSprite')
  request.r(i3673[6], i3673[7], 0, i3672, 'm_DisabledSprite')
  return i3672
}

Deserializers["UnityEngine.UI.AnimationTriggers"] = function (request, data, root) {
  var i3674 = root || request.c( 'UnityEngine.UI.AnimationTriggers' )
  var i3675 = data
  i3674.m_NormalTrigger = i3675[0]
  i3674.m_HighlightedTrigger = i3675[1]
  i3674.m_PressedTrigger = i3675[2]
  i3674.m_SelectedTrigger = i3675[3]
  i3674.m_DisabledTrigger = i3675[4]
  return i3674
}

Deserializers["SoundUIElement"] = function (request, data, root) {
  var i3676 = root || request.c( 'SoundUIElement' )
  var i3677 = data
  i3676.Sound = request.d('SoundDefine', i3677[0], i3676.Sound)
  i3676.PlayOnEnable = !!i3677[1]
  i3676.StopOnDisable = !!i3677[2]
  i3676.playWithInteractable = !!i3677[3]
  i3676.isPlayRandomBackGroundMusic = !!i3677[4]
  return i3676
}

Deserializers["SoundDefine"] = function (request, data, root) {
  var i3678 = root || request.c( 'SoundDefine' )
  var i3679 = data
  i3678.soundType = i3679[0]
  i3678.Loop = !!i3679[1]
  request.r(i3679[2], i3679[3], 0, i3678, 'Clip')
  var i3681 = i3679[4]
  var i3680 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.AudioClip')))
  for(var i = 0; i < i3681.length; i += 2) {
  request.r(i3681[i + 0], i3681[i + 1], 1, i3680, '')
  }
  i3678.ClipList = i3680
  return i3678
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh"] = function (request, data, root) {
  var i3684 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh' )
  var i3685 = data
  i3684.name = i3685[0]
  i3684.halfPrecision = !!i3685[1]
  i3684.useUInt32IndexFormat = !!i3685[2]
  i3684.vertexCount = i3685[3]
  i3684.aabb = i3685[4]
  var i3687 = i3685[5]
  var i3686 = []
  for(var i = 0; i < i3687.length; i += 1) {
    i3686.push( !!i3687[i + 0] );
  }
  i3684.streams = i3686
  i3684.vertices = i3685[6]
  var i3689 = i3685[7]
  var i3688 = []
  for(var i = 0; i < i3689.length; i += 1) {
    i3688.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh', i3689[i + 0]) );
  }
  i3684.subMeshes = i3688
  var i3691 = i3685[8]
  var i3690 = []
  for(var i = 0; i < i3691.length; i += 16) {
    i3690.push( new pc.Mat4().setData(i3691[i + 0], i3691[i + 1], i3691[i + 2], i3691[i + 3],  i3691[i + 4], i3691[i + 5], i3691[i + 6], i3691[i + 7],  i3691[i + 8], i3691[i + 9], i3691[i + 10], i3691[i + 11],  i3691[i + 12], i3691[i + 13], i3691[i + 14], i3691[i + 15]) );
  }
  i3684.bindposes = i3690
  var i3693 = i3685[9]
  var i3692 = []
  for(var i = 0; i < i3693.length; i += 1) {
    i3692.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape', i3693[i + 0]) );
  }
  i3684.blendShapes = i3692
  return i3684
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh"] = function (request, data, root) {
  var i3698 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh' )
  var i3699 = data
  i3698.triangles = i3699[0]
  return i3698
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape"] = function (request, data, root) {
  var i3704 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape' )
  var i3705 = data
  i3704.name = i3705[0]
  var i3707 = i3705[1]
  var i3706 = []
  for(var i = 0; i < i3707.length; i += 1) {
    i3706.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame', i3707[i + 0]) );
  }
  i3704.frames = i3706
  return i3704
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i3708 = root || new pc.UnityMaterial()
  var i3709 = data
  i3708.name = i3709[0]
  request.r(i3709[1], i3709[2], 0, i3708, 'shader')
  i3708.renderQueue = i3709[3]
  i3708.enableInstancing = !!i3709[4]
  var i3711 = i3709[5]
  var i3710 = []
  for(var i = 0; i < i3711.length; i += 1) {
    i3710.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i3711[i + 0]) );
  }
  i3708.floatParameters = i3710
  var i3713 = i3709[6]
  var i3712 = []
  for(var i = 0; i < i3713.length; i += 1) {
    i3712.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i3713[i + 0]) );
  }
  i3708.colorParameters = i3712
  var i3715 = i3709[7]
  var i3714 = []
  for(var i = 0; i < i3715.length; i += 1) {
    i3714.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i3715[i + 0]) );
  }
  i3708.vectorParameters = i3714
  var i3717 = i3709[8]
  var i3716 = []
  for(var i = 0; i < i3717.length; i += 1) {
    i3716.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i3717[i + 0]) );
  }
  i3708.textureParameters = i3716
  var i3719 = i3709[9]
  var i3718 = []
  for(var i = 0; i < i3719.length; i += 1) {
    i3718.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i3719[i + 0]) );
  }
  i3708.materialFlags = i3718
  return i3708
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i3722 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i3723 = data
  i3722.name = i3723[0]
  i3722.value = i3723[1]
  return i3722
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i3726 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i3727 = data
  i3726.name = i3727[0]
  i3726.value = new pc.Color(i3727[1], i3727[2], i3727[3], i3727[4])
  return i3726
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i3730 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i3731 = data
  i3730.name = i3731[0]
  i3730.value = new pc.Vec4( i3731[1], i3731[2], i3731[3], i3731[4] )
  return i3730
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i3734 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i3735 = data
  i3734.name = i3735[0]
  request.r(i3735[1], i3735[2], 0, i3734, 'value')
  return i3734
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i3738 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i3739 = data
  i3738.name = i3739[0]
  i3738.enabled = !!i3739[1]
  return i3738
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i3740 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i3741 = data
  i3740.position = new pc.Vec3( i3741[0], i3741[1], i3741[2] )
  i3740.scale = new pc.Vec3( i3741[3], i3741[4], i3741[5] )
  i3740.rotation = new pc.Quat(i3741[6], i3741[7], i3741[8], i3741[9])
  return i3740
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshFilter"] = function (request, data, root) {
  var i3742 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshFilter' )
  var i3743 = data
  request.r(i3743[0], i3743[1], 0, i3742, 'sharedMesh')
  return i3742
}

Deserializers["CubeTargetControl"] = function (request, data, root) {
  var i3744 = root || request.c( 'CubeTargetControl' )
  var i3745 = data
  var i3747 = i3745[0]
  var i3746 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Transform')))
  for(var i = 0; i < i3747.length; i += 2) {
  request.r(i3747[i + 0], i3747[i + 1], 1, i3746, '')
  }
  i3744.TargetChildren = i3746
  var i3749 = i3745[1]
  var i3748 = []
  for(var i = 0; i < i3749.length; i += 2) {
  request.r(i3749[i + 0], i3749[i + 1], 2, i3748, '')
  }
  i3744.MeshRenderer = i3748
  i3744.IsActive = !!i3745[2]
  i3744.RollWoolTime = i3745[3]
  i3744.DelayTime = i3745[4]
  i3744.VibrationStrength = i3745[5]
  request.r(i3745[6], i3745[7], 0, i3744, 'AddCubeIcon')
  request.r(i3745[8], i3745[9], 0, i3744, '_boxAnimation')
  i3744._boxMoveAnimation = i3745[10]
  request.r(i3745[11], i3745[12], 0, i3744, '_boxCollider')
  return i3744
}

Deserializers["TargetBoxAnimation"] = function (request, data, root) {
  var i3754 = root || request.c( 'TargetBoxAnimation' )
  var i3755 = data
  i3754._boxMoveTime = i3755[0]
  i3754._capMoveTime = i3755[1]
  i3754._hopDownTime = i3755[2]
  i3754._hopUpTime = i3755[3]
  request.r(i3755[4], i3755[5], 0, i3754, 'boxWhooshClip')
  request.r(i3755[6], i3755[7], 0, i3754, '_cap')
  request.r(i3755[8], i3755[9], 0, i3754, '_closeParticle')
  i3754._boxMoveDistance = i3755[10]
  i3754._capMoveDistance = i3755[11]
  i3754._capScaleTime = i3755[12]
  i3754._hopScale = i3755[13]
  i3754.offsetMoveY = i3755[14]
  return i3754
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.BoxCollider"] = function (request, data, root) {
  var i3756 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.BoxCollider' )
  var i3757 = data
  i3756.center = new pc.Vec3( i3757[0], i3757[1], i3757[2] )
  i3756.size = new pc.Vec3( i3757[3], i3757[4], i3757[5] )
  i3756.enabled = !!i3757[6]
  i3756.isTrigger = !!i3757[7]
  request.r(i3757[8], i3757[9], 0, i3756, 'material')
  return i3756
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshRenderer"] = function (request, data, root) {
  var i3758 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshRenderer' )
  var i3759 = data
  request.r(i3759[0], i3759[1], 0, i3758, 'additionalVertexStreams')
  i3758.enabled = !!i3759[2]
  request.r(i3759[3], i3759[4], 0, i3758, 'sharedMaterial')
  var i3761 = i3759[5]
  var i3760 = []
  for(var i = 0; i < i3761.length; i += 2) {
  request.r(i3761[i + 0], i3761[i + 1], 2, i3760, '')
  }
  i3758.sharedMaterials = i3760
  i3758.receiveShadows = !!i3759[6]
  i3758.shadowCastingMode = i3759[7]
  i3758.sortingLayerID = i3759[8]
  i3758.sortingOrder = i3759[9]
  i3758.lightmapIndex = i3759[10]
  i3758.lightmapSceneIndex = i3759[11]
  i3758.lightmapScaleOffset = new pc.Vec4( i3759[12], i3759[13], i3759[14], i3759[15] )
  i3758.lightProbeUsage = i3759[16]
  i3758.reflectionProbeUsage = i3759[17]
  return i3758
}

Deserializers["QueueTargetControl"] = function (request, data, root) {
  var i3764 = root || request.c( 'QueueTargetControl' )
  var i3765 = data
  return i3764
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystem"] = function (request, data, root) {
  var i3766 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystem' )
  var i3767 = data
  i3766.main = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule', i3767[0], i3766.main)
  i3766.colorBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule', i3767[1], i3766.colorBySpeed)
  i3766.colorOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule', i3767[2], i3766.colorOverLifetime)
  i3766.emission = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule', i3767[3], i3766.emission)
  i3766.rotationBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule', i3767[4], i3766.rotationBySpeed)
  i3766.rotationOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule', i3767[5], i3766.rotationOverLifetime)
  i3766.shape = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule', i3767[6], i3766.shape)
  i3766.sizeBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule', i3767[7], i3766.sizeBySpeed)
  i3766.sizeOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule', i3767[8], i3766.sizeOverLifetime)
  i3766.textureSheetAnimation = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule', i3767[9], i3766.textureSheetAnimation)
  i3766.velocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule', i3767[10], i3766.velocityOverLifetime)
  i3766.noise = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule', i3767[11], i3766.noise)
  i3766.inheritVelocity = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule', i3767[12], i3766.inheritVelocity)
  i3766.forceOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule', i3767[13], i3766.forceOverLifetime)
  i3766.limitVelocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule', i3767[14], i3766.limitVelocityOverLifetime)
  i3766.useAutoRandomSeed = !!i3767[15]
  i3766.randomSeed = i3767[16]
  return i3766
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule"] = function (request, data, root) {
  var i3768 = root || new pc.ParticleSystemMain()
  var i3769 = data
  i3768.duration = i3769[0]
  i3768.loop = !!i3769[1]
  i3768.prewarm = !!i3769[2]
  i3768.startDelay = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[3], i3768.startDelay)
  i3768.startLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[4], i3768.startLifetime)
  i3768.startSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[5], i3768.startSpeed)
  i3768.startSize3D = !!i3769[6]
  i3768.startSizeX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[7], i3768.startSizeX)
  i3768.startSizeY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[8], i3768.startSizeY)
  i3768.startSizeZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[9], i3768.startSizeZ)
  i3768.startRotation3D = !!i3769[10]
  i3768.startRotationX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[11], i3768.startRotationX)
  i3768.startRotationY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[12], i3768.startRotationY)
  i3768.startRotationZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[13], i3768.startRotationZ)
  i3768.startColor = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3769[14], i3768.startColor)
  i3768.gravityModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3769[15], i3768.gravityModifier)
  i3768.simulationSpace = i3769[16]
  request.r(i3769[17], i3769[18], 0, i3768, 'customSimulationSpace')
  i3768.simulationSpeed = i3769[19]
  i3768.useUnscaledTime = !!i3769[20]
  i3768.scalingMode = i3769[21]
  i3768.playOnAwake = !!i3769[22]
  i3768.maxParticles = i3769[23]
  i3768.emitterVelocityMode = i3769[24]
  i3768.stopAction = i3769[25]
  return i3768
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve"] = function (request, data, root) {
  var i3770 = root || new pc.MinMaxCurve()
  var i3771 = data
  i3770.mode = i3771[0]
  i3770.curveMin = new pc.AnimationCurve( { keys_flow: i3771[1] } )
  i3770.curveMax = new pc.AnimationCurve( { keys_flow: i3771[2] } )
  i3770.curveMultiplier = i3771[3]
  i3770.constantMin = i3771[4]
  i3770.constantMax = i3771[5]
  return i3770
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient"] = function (request, data, root) {
  var i3772 = root || new pc.MinMaxGradient()
  var i3773 = data
  i3772.mode = i3773[0]
  i3772.gradientMin = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i3773[1], i3772.gradientMin)
  i3772.gradientMax = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i3773[2], i3772.gradientMax)
  i3772.colorMin = new pc.Color(i3773[3], i3773[4], i3773[5], i3773[6])
  i3772.colorMax = new pc.Color(i3773[7], i3773[8], i3773[9], i3773[10])
  return i3772
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient"] = function (request, data, root) {
  var i3774 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient' )
  var i3775 = data
  i3774.mode = i3775[0]
  var i3777 = i3775[1]
  var i3776 = []
  for(var i = 0; i < i3777.length; i += 1) {
    i3776.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey', i3777[i + 0]) );
  }
  i3774.colorKeys = i3776
  var i3779 = i3775[2]
  var i3778 = []
  for(var i = 0; i < i3779.length; i += 1) {
    i3778.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey', i3779[i + 0]) );
  }
  i3774.alphaKeys = i3778
  return i3774
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule"] = function (request, data, root) {
  var i3780 = root || new pc.ParticleSystemColorBySpeed()
  var i3781 = data
  i3780.enabled = !!i3781[0]
  i3780.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3781[1], i3780.color)
  i3780.range = new pc.Vec2( i3781[2], i3781[3] )
  return i3780
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey"] = function (request, data, root) {
  var i3784 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey' )
  var i3785 = data
  i3784.color = new pc.Color(i3785[0], i3785[1], i3785[2], i3785[3])
  i3784.time = i3785[4]
  return i3784
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey"] = function (request, data, root) {
  var i3788 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey' )
  var i3789 = data
  i3788.alpha = i3789[0]
  i3788.time = i3789[1]
  return i3788
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule"] = function (request, data, root) {
  var i3790 = root || new pc.ParticleSystemColorOverLifetime()
  var i3791 = data
  i3790.enabled = !!i3791[0]
  i3790.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3791[1], i3790.color)
  return i3790
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule"] = function (request, data, root) {
  var i3792 = root || new pc.ParticleSystemEmitter()
  var i3793 = data
  i3792.enabled = !!i3793[0]
  i3792.rateOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3793[1], i3792.rateOverTime)
  i3792.rateOverDistance = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3793[2], i3792.rateOverDistance)
  var i3795 = i3793[3]
  var i3794 = []
  for(var i = 0; i < i3795.length; i += 1) {
    i3794.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst', i3795[i + 0]) );
  }
  i3792.bursts = i3794
  return i3792
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst"] = function (request, data, root) {
  var i3798 = root || new pc.ParticleSystemBurst()
  var i3799 = data
  i3798.count = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3799[0], i3798.count)
  i3798.cycleCount = i3799[1]
  i3798.minCount = i3799[2]
  i3798.maxCount = i3799[3]
  i3798.repeatInterval = i3799[4]
  i3798.time = i3799[5]
  return i3798
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule"] = function (request, data, root) {
  var i3800 = root || new pc.ParticleSystemRotationBySpeed()
  var i3801 = data
  i3800.enabled = !!i3801[0]
  i3800.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3801[1], i3800.x)
  i3800.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3801[2], i3800.y)
  i3800.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3801[3], i3800.z)
  i3800.separateAxes = !!i3801[4]
  i3800.range = new pc.Vec2( i3801[5], i3801[6] )
  return i3800
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule"] = function (request, data, root) {
  var i3802 = root || new pc.ParticleSystemRotationOverLifetime()
  var i3803 = data
  i3802.enabled = !!i3803[0]
  i3802.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3803[1], i3802.x)
  i3802.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3803[2], i3802.y)
  i3802.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3803[3], i3802.z)
  i3802.separateAxes = !!i3803[4]
  return i3802
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule"] = function (request, data, root) {
  var i3804 = root || new pc.ParticleSystemShape()
  var i3805 = data
  i3804.enabled = !!i3805[0]
  i3804.shapeType = i3805[1]
  i3804.randomDirectionAmount = i3805[2]
  i3804.sphericalDirectionAmount = i3805[3]
  i3804.randomPositionAmount = i3805[4]
  i3804.alignToDirection = !!i3805[5]
  i3804.radius = i3805[6]
  i3804.radiusMode = i3805[7]
  i3804.radiusSpread = i3805[8]
  i3804.radiusSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3805[9], i3804.radiusSpeed)
  i3804.radiusThickness = i3805[10]
  i3804.angle = i3805[11]
  i3804.length = i3805[12]
  i3804.boxThickness = new pc.Vec3( i3805[13], i3805[14], i3805[15] )
  i3804.meshShapeType = i3805[16]
  request.r(i3805[17], i3805[18], 0, i3804, 'mesh')
  request.r(i3805[19], i3805[20], 0, i3804, 'meshRenderer')
  request.r(i3805[21], i3805[22], 0, i3804, 'skinnedMeshRenderer')
  i3804.useMeshMaterialIndex = !!i3805[23]
  i3804.meshMaterialIndex = i3805[24]
  i3804.useMeshColors = !!i3805[25]
  i3804.normalOffset = i3805[26]
  i3804.arc = i3805[27]
  i3804.arcMode = i3805[28]
  i3804.arcSpread = i3805[29]
  i3804.arcSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3805[30], i3804.arcSpeed)
  i3804.donutRadius = i3805[31]
  i3804.position = new pc.Vec3( i3805[32], i3805[33], i3805[34] )
  i3804.rotation = new pc.Vec3( i3805[35], i3805[36], i3805[37] )
  i3804.scale = new pc.Vec3( i3805[38], i3805[39], i3805[40] )
  return i3804
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule"] = function (request, data, root) {
  var i3806 = root || new pc.ParticleSystemSizeBySpeed()
  var i3807 = data
  i3806.enabled = !!i3807[0]
  i3806.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3807[1], i3806.x)
  i3806.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3807[2], i3806.y)
  i3806.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3807[3], i3806.z)
  i3806.separateAxes = !!i3807[4]
  i3806.range = new pc.Vec2( i3807[5], i3807[6] )
  return i3806
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule"] = function (request, data, root) {
  var i3808 = root || new pc.ParticleSystemSizeOverLifetime()
  var i3809 = data
  i3808.enabled = !!i3809[0]
  i3808.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3809[1], i3808.x)
  i3808.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3809[2], i3808.y)
  i3808.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3809[3], i3808.z)
  i3808.separateAxes = !!i3809[4]
  return i3808
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule"] = function (request, data, root) {
  var i3810 = root || new pc.ParticleSystemTextureSheetAnimation()
  var i3811 = data
  i3810.enabled = !!i3811[0]
  i3810.mode = i3811[1]
  i3810.animation = i3811[2]
  i3810.numTilesX = i3811[3]
  i3810.numTilesY = i3811[4]
  i3810.useRandomRow = !!i3811[5]
  i3810.frameOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3811[6], i3810.frameOverTime)
  i3810.startFrame = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3811[7], i3810.startFrame)
  i3810.cycleCount = i3811[8]
  i3810.rowIndex = i3811[9]
  i3810.flipU = i3811[10]
  i3810.flipV = i3811[11]
  i3810.spriteCount = i3811[12]
  var i3813 = i3811[13]
  var i3812 = []
  for(var i = 0; i < i3813.length; i += 2) {
  request.r(i3813[i + 0], i3813[i + 1], 2, i3812, '')
  }
  i3810.sprites = i3812
  return i3810
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule"] = function (request, data, root) {
  var i3816 = root || new pc.ParticleSystemVelocityOverLifetime()
  var i3817 = data
  i3816.enabled = !!i3817[0]
  i3816.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[1], i3816.x)
  i3816.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[2], i3816.y)
  i3816.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[3], i3816.z)
  i3816.radial = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[4], i3816.radial)
  i3816.speedModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[5], i3816.speedModifier)
  i3816.space = i3817[6]
  i3816.orbitalX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[7], i3816.orbitalX)
  i3816.orbitalY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[8], i3816.orbitalY)
  i3816.orbitalZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[9], i3816.orbitalZ)
  i3816.orbitalOffsetX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[10], i3816.orbitalOffsetX)
  i3816.orbitalOffsetY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[11], i3816.orbitalOffsetY)
  i3816.orbitalOffsetZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3817[12], i3816.orbitalOffsetZ)
  return i3816
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule"] = function (request, data, root) {
  var i3818 = root || new pc.ParticleSystemNoise()
  var i3819 = data
  i3818.enabled = !!i3819[0]
  i3818.separateAxes = !!i3819[1]
  i3818.strengthX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[2], i3818.strengthX)
  i3818.strengthY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[3], i3818.strengthY)
  i3818.strengthZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[4], i3818.strengthZ)
  i3818.frequency = i3819[5]
  i3818.damping = !!i3819[6]
  i3818.octaveCount = i3819[7]
  i3818.octaveMultiplier = i3819[8]
  i3818.octaveScale = i3819[9]
  i3818.quality = i3819[10]
  i3818.scrollSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[11], i3818.scrollSpeed)
  i3818.scrollSpeedMultiplier = i3819[12]
  i3818.remapEnabled = !!i3819[13]
  i3818.remapX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[14], i3818.remapX)
  i3818.remapY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[15], i3818.remapY)
  i3818.remapZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[16], i3818.remapZ)
  i3818.positionAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[17], i3818.positionAmount)
  i3818.rotationAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[18], i3818.rotationAmount)
  i3818.sizeAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3819[19], i3818.sizeAmount)
  return i3818
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule"] = function (request, data, root) {
  var i3820 = root || new pc.ParticleSystemInheritVelocity()
  var i3821 = data
  i3820.enabled = !!i3821[0]
  i3820.mode = i3821[1]
  i3820.curve = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3821[2], i3820.curve)
  return i3820
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule"] = function (request, data, root) {
  var i3822 = root || new pc.ParticleSystemForceOverLifetime()
  var i3823 = data
  i3822.enabled = !!i3823[0]
  i3822.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3823[1], i3822.x)
  i3822.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3823[2], i3822.y)
  i3822.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3823[3], i3822.z)
  i3822.space = i3823[4]
  i3822.randomized = !!i3823[5]
  return i3822
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule"] = function (request, data, root) {
  var i3824 = root || new pc.ParticleSystemLimitVelocityOverLifetime()
  var i3825 = data
  i3824.enabled = !!i3825[0]
  i3824.limit = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3825[1], i3824.limit)
  i3824.limitX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3825[2], i3824.limitX)
  i3824.limitY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3825[3], i3824.limitY)
  i3824.limitZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3825[4], i3824.limitZ)
  i3824.dampen = i3825[5]
  i3824.separateAxes = !!i3825[6]
  i3824.space = i3825[7]
  i3824.drag = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3825[8], i3824.drag)
  i3824.multiplyDragByParticleSize = !!i3825[9]
  i3824.multiplyDragByParticleVelocity = !!i3825[10]
  return i3824
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer"] = function (request, data, root) {
  var i3826 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer' )
  var i3827 = data
  i3826.enabled = !!i3827[0]
  request.r(i3827[1], i3827[2], 0, i3826, 'sharedMaterial')
  var i3829 = i3827[3]
  var i3828 = []
  for(var i = 0; i < i3829.length; i += 2) {
  request.r(i3829[i + 0], i3829[i + 1], 2, i3828, '')
  }
  i3826.sharedMaterials = i3828
  i3826.receiveShadows = !!i3827[4]
  i3826.shadowCastingMode = i3827[5]
  i3826.sortingLayerID = i3827[6]
  i3826.sortingOrder = i3827[7]
  i3826.lightmapIndex = i3827[8]
  i3826.lightmapSceneIndex = i3827[9]
  i3826.lightmapScaleOffset = new pc.Vec4( i3827[10], i3827[11], i3827[12], i3827[13] )
  i3826.lightProbeUsage = i3827[14]
  i3826.reflectionProbeUsage = i3827[15]
  request.r(i3827[16], i3827[17], 0, i3826, 'mesh')
  i3826.meshCount = i3827[18]
  i3826.activeVertexStreamsCount = i3827[19]
  i3826.alignment = i3827[20]
  i3826.renderMode = i3827[21]
  i3826.sortMode = i3827[22]
  i3826.lengthScale = i3827[23]
  i3826.velocityScale = i3827[24]
  i3826.cameraVelocityScale = i3827[25]
  i3826.normalDirection = i3827[26]
  i3826.sortingFudge = i3827[27]
  i3826.minParticleSize = i3827[28]
  i3826.maxParticleSize = i3827[29]
  i3826.pivot = new pc.Vec3( i3827[30], i3827[31], i3827[32] )
  request.r(i3827[33], i3827[34], 0, i3826, 'trailMaterial')
  return i3826
}

Deserializers["HandController"] = function (request, data, root) {
  var i3830 = root || request.c( 'HandController' )
  var i3831 = data
  var i3833 = i3831[0]
  var i3832 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Sprite')))
  for(var i = 0; i < i3833.length; i += 2) {
  request.r(i3833[i + 0], i3833[i + 1], 1, i3832, '')
  }
  i3830.handSprites = i3832
  request.r(i3831[1], i3831[2], 0, i3830, 'handSpriteRenderer')
  i3830.positionShow = new pc.Vec3( i3831[3], i3831[4], i3831[5] )
  i3830.positionHide = new pc.Vec3( i3831[6], i3831[7], i3831[8] )
  i3830.delayTime = i3831[9]
  return i3830
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer"] = function (request, data, root) {
  var i3836 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer' )
  var i3837 = data
  i3836.enabled = !!i3837[0]
  request.r(i3837[1], i3837[2], 0, i3836, 'sharedMaterial')
  var i3839 = i3837[3]
  var i3838 = []
  for(var i = 0; i < i3839.length; i += 2) {
  request.r(i3839[i + 0], i3839[i + 1], 2, i3838, '')
  }
  i3836.sharedMaterials = i3838
  i3836.receiveShadows = !!i3837[4]
  i3836.shadowCastingMode = i3837[5]
  i3836.sortingLayerID = i3837[6]
  i3836.sortingOrder = i3837[7]
  i3836.lightmapIndex = i3837[8]
  i3836.lightmapSceneIndex = i3837[9]
  i3836.lightmapScaleOffset = new pc.Vec4( i3837[10], i3837[11], i3837[12], i3837[13] )
  i3836.lightProbeUsage = i3837[14]
  i3836.reflectionProbeUsage = i3837[15]
  i3836.color = new pc.Color(i3837[16], i3837[17], i3837[18], i3837[19])
  request.r(i3837[20], i3837[21], 0, i3836, 'sprite')
  i3836.flipX = !!i3837[22]
  i3836.flipY = !!i3837[23]
  i3836.drawMode = i3837[24]
  i3836.size = new pc.Vec2( i3837[25], i3837[26] )
  i3836.tileMode = i3837[27]
  i3836.adaptiveModeThreshold = i3837[28]
  i3836.maskInteraction = i3837[29]
  i3836.spriteSortPoint = i3837[30]
  return i3836
}

Deserializers["YarnWoolAnimation"] = function (request, data, root) {
  var i3840 = root || request.c( 'YarnWoolAnimation' )
  var i3841 = data
  request.r(i3841[0], i3841[1], 0, i3840, 'WoolAnimationData')
  request.r(i3841[2], i3841[3], 0, i3840, 'LineRenderer')
  return i3840
}

Deserializers["ObjectPool_Effect"] = function (request, data, root) {
  var i3842 = root || request.c( 'ObjectPool_Effect' )
  var i3843 = data
  i3842.waitTime = i3843[0]
  return i3842
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.LineRenderer"] = function (request, data, root) {
  var i3844 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.LineRenderer' )
  var i3845 = data
  i3844.textureMode = i3845[0]
  i3844.alignment = i3845[1]
  i3844.widthCurve = new pc.AnimationCurve( { keys_flow: i3845[2] } )
  i3844.colorGradient = i3845[3] ? new pc.ColorGradient(i3845[3][0], i3845[3][1], i3845[3][2]) : null
  var i3847 = i3845[4]
  var i3846 = []
  for(var i = 0; i < i3847.length; i += 3) {
    i3846.push( new pc.Vec3( i3847[i + 0], i3847[i + 1], i3847[i + 2] ) );
  }
  i3844.positions = i3846
  i3844.positionCount = i3845[5]
  i3844.widthMultiplier = i3845[6]
  i3844.startWidth = i3845[7]
  i3844.endWidth = i3845[8]
  i3844.numCornerVertices = i3845[9]
  i3844.numCapVertices = i3845[10]
  i3844.useWorldSpace = !!i3845[11]
  i3844.loop = !!i3845[12]
  i3844.startColor = new pc.Color(i3845[13], i3845[14], i3845[15], i3845[16])
  i3844.endColor = new pc.Color(i3845[17], i3845[18], i3845[19], i3845[20])
  i3844.generateLightingData = !!i3845[21]
  i3844.enabled = !!i3845[22]
  request.r(i3845[23], i3845[24], 0, i3844, 'sharedMaterial')
  var i3849 = i3845[25]
  var i3848 = []
  for(var i = 0; i < i3849.length; i += 2) {
  request.r(i3849[i + 0], i3849[i + 1], 2, i3848, '')
  }
  i3844.sharedMaterials = i3848
  i3844.receiveShadows = !!i3845[26]
  i3844.shadowCastingMode = i3845[27]
  i3844.sortingLayerID = i3845[28]
  i3844.sortingOrder = i3845[29]
  i3844.lightmapIndex = i3845[30]
  i3844.lightmapSceneIndex = i3845[31]
  i3844.lightmapScaleOffset = new pc.Vec4( i3845[32], i3845[33], i3845[34], i3845[35] )
  i3844.lightProbeUsage = i3845[36]
  i3844.reflectionProbeUsage = i3845[37]
  return i3844
}

Deserializers["RollWoolAnimation"] = function (request, data, root) {
  var i3852 = root || request.c( 'RollWoolAnimation' )
  var i3853 = data
  request.r(i3853[0], i3853[1], 0, i3852, 'woolClip1')
  request.r(i3853[2], i3853[3], 0, i3852, 'woolClip2')
  request.r(i3853[4], i3853[5], 0, i3852, 'WoolAnimationData')
  var i3855 = i3853[6]
  var i3854 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.MeshRenderer')))
  for(var i = 0; i < i3855.length; i += 2) {
  request.r(i3855[i + 0], i3855[i + 1], 1, i3854, '')
  }
  i3852.MeshRenderers = i3854
  i3852._localScale = new pc.Vec3( i3853[7], i3853[8], i3853[9] )
  return i3852
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Cubemap"] = function (request, data, root) {
  var i3858 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Cubemap' )
  var i3859 = data
  i3858.name = i3859[0]
  i3858.atlasId = i3859[1]
  i3858.mipmapCount = i3859[2]
  i3858.hdr = !!i3859[3]
  i3858.size = i3859[4]
  i3858.anisoLevel = i3859[5]
  i3858.filterMode = i3859[6]
  var i3861 = i3859[7]
  var i3860 = []
  for(var i = 0; i < i3861.length; i += 4) {
    i3860.push( UnityEngine.Rect.MinMaxRect(i3861[i + 0], i3861[i + 1], i3861[i + 2], i3861[i + 3]) );
  }
  i3858.rects = i3860
  i3858.wrapU = i3859[8]
  i3858.wrapV = i3859[9]
  return i3858
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i3864 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i3865 = data
  i3864.name = i3865[0]
  i3864.index = i3865[1]
  i3864.startup = !!i3865[2]
  return i3864
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Light"] = function (request, data, root) {
  var i3866 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Light' )
  var i3867 = data
  i3866.enabled = !!i3867[0]
  i3866.type = i3867[1]
  i3866.color = new pc.Color(i3867[2], i3867[3], i3867[4], i3867[5])
  i3866.cullingMask = i3867[6]
  i3866.intensity = i3867[7]
  i3866.range = i3867[8]
  i3866.spotAngle = i3867[9]
  i3866.shadows = i3867[10]
  i3866.shadowNormalBias = i3867[11]
  i3866.shadowBias = i3867[12]
  i3866.shadowStrength = i3867[13]
  i3866.shadowResolution = i3867[14]
  i3866.lightmapBakeType = i3867[15]
  i3866.renderMode = i3867[16]
  request.r(i3867[17], i3867[18], 0, i3866, 'cookie')
  i3866.cookieSize = i3867[19]
  return i3866
}

Deserializers["UnityEngine.UI.CanvasScaler"] = function (request, data, root) {
  var i3868 = root || request.c( 'UnityEngine.UI.CanvasScaler' )
  var i3869 = data
  i3868.m_UiScaleMode = i3869[0]
  i3868.m_ReferencePixelsPerUnit = i3869[1]
  i3868.m_ScaleFactor = i3869[2]
  i3868.m_ReferenceResolution = new pc.Vec2( i3869[3], i3869[4] )
  i3868.m_ScreenMatchMode = i3869[5]
  i3868.m_MatchWidthOrHeight = i3869[6]
  i3868.m_PhysicalUnit = i3869[7]
  i3868.m_FallbackScreenDPI = i3869[8]
  i3868.m_DefaultSpriteDPI = i3869[9]
  i3868.m_DynamicPixelsPerUnit = i3869[10]
  i3868.m_PresetInfoIsWorld = !!i3869[11]
  return i3868
}

Deserializers["Interactable"] = function (request, data, root) {
  var i3870 = root || request.c( 'Interactable' )
  var i3871 = data
  i3870.HoldThreshold = i3871[0]
  i3870.SwipeThreshold = i3871[1]
  return i3870
}

Deserializers["PlayNowButtonAnim"] = function (request, data, root) {
  var i3872 = root || request.c( 'PlayNowButtonAnim' )
  var i3873 = data
  request.r(i3873[0], i3873[1], 0, i3872, 'playerNowButton')
  i3872.maxScale = new pc.Vec3( i3873[2], i3873[3], i3873[4] )
  i3872.minScale = new pc.Vec3( i3873[5], i3873[6], i3873[7] )
  i3872.scaleDuration = i3873[8]
  return i3872
}

Deserializers["UnityEngine.UI.Outline"] = function (request, data, root) {
  var i3874 = root || request.c( 'UnityEngine.UI.Outline' )
  var i3875 = data
  i3874.m_EffectColor = new pc.Color(i3875[0], i3875[1], i3875[2], i3875[3])
  i3874.m_EffectDistance = new pc.Vec2( i3875[4], i3875[5] )
  i3874.m_UseGraphicAlpha = !!i3875[6]
  return i3874
}

Deserializers["UnityEngine.UI.Shadow"] = function (request, data, root) {
  var i3876 = root || request.c( 'UnityEngine.UI.Shadow' )
  var i3877 = data
  i3876.m_EffectColor = new pc.Color(i3877[0], i3877[1], i3877[2], i3877[3])
  i3876.m_EffectDistance = new pc.Vec2( i3877[4], i3877[5] )
  i3876.m_UseGraphicAlpha = !!i3877[6]
  return i3876
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.AudioSource"] = function (request, data, root) {
  var i3878 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.AudioSource' )
  var i3879 = data
  request.r(i3879[0], i3879[1], 0, i3878, 'clip')
  request.r(i3879[2], i3879[3], 0, i3878, 'outputAudioMixerGroup')
  i3878.playOnAwake = !!i3879[4]
  i3878.loop = !!i3879[5]
  i3878.time = i3879[6]
  i3878.volume = i3879[7]
  i3878.pitch = i3879[8]
  i3878.enabled = !!i3879[9]
  return i3878
}

Deserializers["CameraController"] = function (request, data, root) {
  var i3880 = root || request.c( 'CameraController' )
  var i3881 = data
  request.r(i3881[0], i3881[1], 0, i3880, 'InputInteractable')
  request.r(i3881[2], i3881[3], 0, i3880, 'ZoomCameraData')
  request.r(i3881[4], i3881[5], 0, i3880, 'BackGround')
  request.r(i3881[6], i3881[7], 0, i3880, 'SpawnPoint')
  request.r(i3881[8], i3881[9], 0, i3880, 'ModelPrefab')
  i3880.targetRotation = new pc.Quat(i3881[10], i3881[11], i3881[12], i3881[13])
  i3880.Friction = i3881[14]
  i3880.RotationSensitivity = new pc.Vec2( i3881[15], i3881[16] )
  i3880.AccelerationRange = new pc.Vec2( i3881[17], i3881[18] )
  i3880.RotationSpeed = i3881[19]
  i3880.RotationAutoSpeed = i3881[20]
  i3880.SmoothingTime = i3881[21]
  i3880.TimeAFKToAutoRotation = i3881[22]
  i3880.IntroLenght = i3881[23]
  i3880.ModelRotationIntroSpeed = i3881[24]
  i3880.IntroCameraZoomInDuration = i3881[25]
  i3880.IntroStartFOV = i3881[26]
  i3880.IntroEndFOV = i3881[27]
  i3880.DragStyle = i3881[28]
  i3880.DraggingSpeed = i3881[29]
  i3880.SmoothFactor = i3881[30]
  i3880.ZoomStyle = i3881[31]
  i3880._cameraPosMainMenuDefault = new pc.Vec3( i3881[32], i3881[33], i3881[34] )
  i3880._cameraRoteMainMenuDefault = new pc.Vec3( i3881[35], i3881[36], i3881[37] )
  i3880._cameraPosGamePlayDefault = new pc.Vec3( i3881[38], i3881[39], i3881[40] )
  i3880._cameraRoteGamePlayDefault = new pc.Vec3( i3881[41], i3881[42], i3881[43] )
  return i3880
}

Deserializers["GamePlaySystem"] = function (request, data, root) {
  var i3882 = root || request.c( 'GamePlaySystem' )
  var i3883 = data
  request.r(i3883[0], i3883[1], 0, i3882, 'CameraController')
  request.r(i3883[2], i3883[3], 0, i3882, 'BoxChainReaction3D')
  var i3885 = i3883[4]
  var i3884 = new (System.Collections.Generic.List$1(Bridge.ns('CubeTargetControl')))
  for(var i = 0; i < i3885.length; i += 2) {
  request.r(i3885[i + 0], i3885[i + 1], 1, i3884, '')
  }
  i3882.CurrentCubeTargets = i3884
  var i3887 = i3883[5]
  var i3886 = new (System.Collections.Generic.List$1(Bridge.ns('QueueTargetControl')))
  for(var i = 0; i < i3887.length; i += 2) {
  request.r(i3887[i + 0], i3887[i + 1], 1, i3886, '')
  }
  i3882.CurrentQueueTargets = i3886
  request.r(i3883[6], i3883[7], 0, i3882, 'YarnWoolPrefab')
  request.r(i3883[8], i3883[9], 0, i3882, 'RollWoolPrefab')
  i3882.TotalCubeActive = i3883[10]
  i3882.CubeReadyCount = i3883[11]
  request.r(i3883[12], i3883[13], 0, i3882, 'woolXoayClip')
  request.r(i3883[14], i3883[15], 0, i3882, 'wool1Clip')
  request.r(i3883[16], i3883[17], 0, i3882, 'loseSound')
  i3882.IsGoToStore = !!i3883[18]
  request.r(i3883[19], i3883[20], 0, i3882, 'handController')
  i3882.cubeCountClaimed = i3883[21]
  request.r(i3883[22], i3883[23], 0, i3882, 'endGamePanel')
  i3882.LoseOffer = i3883[24]
  i3882._cubeTargetCountDefault = i3883[25]
  request.r(i3883[26], i3883[27], 0, i3882, '_levelPrefab')
  i3882.spacingCubeTarget = i3883[28]
  return i3882
}

Deserializers["ObjectPool"] = function (request, data, root) {
  var i3892 = root || request.c( 'ObjectPool' )
  var i3893 = data
  return i3892
}

Deserializers["SoundManager"] = function (request, data, root) {
  var i3894 = root || request.c( 'SoundManager' )
  var i3895 = data
  request.r(i3895[0], i3895[1], 0, i3894, 'audioMixer')
  request.r(i3895[2], i3895[3], 0, i3894, 'fxMusicSource')
  request.r(i3895[4], i3895[5], 0, i3894, 'specialBgmSource')
  i3894.BGM = request.d('SoundDefine', i3895[6], i3894.BGM)
  return i3894
}

Deserializers["BoxChainReaction3D"] = function (request, data, root) {
  var i3896 = root || request.c( 'BoxChainReaction3D' )
  var i3897 = data
  i3896.InitialBoxCount = i3897[0]
  request.r(i3897[1], i3897[2], 0, i3896, 'boosterWhoosh1Clip')
  request.r(i3897[3], i3897[4], 0, i3896, 'boosterImpact1Clip')
  request.r(i3897[5], i3897[6], 0, i3896, 'boxContainer')
  request.r(i3897[7], i3897[8], 0, i3896, 'boxPrefab')
  i3896.initialSpacing = i3897[9]
  i3896.boxWidth = i3897[10]
  i3896.flyInDuration = i3897[11]
  i3896.collisionDuration = i3897[12]
  i3896.repositionDuration = i3897[13]
  i3896.collisionOffset = i3897[14]
  i3896.flyInStartPosition = new pc.Vec3( i3897[15], i3897[16], i3897[17] )
  i3896.flyInStretch = i3897[18]
  i3896.landingSquash = i3897[19]
  i3896.collisionSquash = i3897[20]
  i3896.collisionStretch = i3897[21]
  return i3896
}

Deserializers["GamePlayMeshController"] = function (request, data, root) {
  var i3898 = root || request.c( 'GamePlayMeshController' )
  var i3899 = data
  i3898.LevelId = i3899[0]
  i3898.IrgnoreLevelId = !!i3899[1]
  i3898.LevelData = request.d('LevelData', i3899[2], i3898.LevelData)
  var i3901 = i3899[3]
  var i3900 = new (System.Collections.Generic.List$1(Bridge.ns('WoolControl')))
  for(var i = 0; i < i3901.length; i += 2) {
  request.r(i3901[i + 0], i3901[i + 1], 1, i3900, '')
  }
  i3898.WoolControls = i3900
  request.r(i3899[4], i3899[5], 0, i3898, 'InterestCurveData')
  request.r(i3899[6], i3899[7], 0, i3898, 'WoolAnimationData')
  request.r(i3899[8], i3899[9], 0, i3898, 'WoolMaterial')
  request.r(i3899[10], i3899[11], 0, i3898, 'WoolChildMaterial')
  i3898.MaxLayerHasThreeSameColor = i3899[12]
  request.r(i3899[13], i3899[14], 0, i3898, 'CenterTransform')
  i3898._maxDistanceFromCetner = i3899[15]
  return i3898
}

Deserializers["LevelData"] = function (request, data, root) {
  var i3902 = root || request.c( 'LevelData' )
  var i3903 = data
  i3902.LevelId = i3903[0]
  i3902.CurrentcyLevel = i3903[1]
  i3902.DynamicDif = i3903[2]
  var i3905 = i3903[3]
  var i3904 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i3905.length; i += 4) {
    i3904.add(new pc.Color(i3905[i + 0], i3905[i + 1], i3905[i + 2], i3905[i + 3]));
  }
  i3902.ColorList = i3904
  var i3907 = i3903[4]
  var i3906 = new (System.Collections.Generic.List$1(Bridge.ns('System.Int32')))
  for(var i = 0; i < i3907.length; i += 1) {
    i3906.add(i3907[i + 0]);
  }
  i3902.ColorCountList = i3906
  return i3902
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshCollider"] = function (request, data, root) {
  var i3914 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshCollider' )
  var i3915 = data
  i3914.enabled = !!i3915[0]
  i3914.isTrigger = !!i3915[1]
  request.r(i3915[2], i3915[3], 0, i3914, 'material')
  request.r(i3915[4], i3915[5], 0, i3914, 'sharedMesh')
  i3914.convex = !!i3915[6]
  return i3914
}

Deserializers["WoolControl"] = function (request, data, root) {
  var i3916 = root || request.c( 'WoolControl' )
  var i3917 = data
  i3916.debugUV = !!i3917[0]
  i3916.MeshObjectData = request.d('MeshObjectData', i3917[1], i3916.MeshObjectData)
  request.r(i3917[2], i3917[3], 0, i3916, 'TopMeshRenderer')
  request.r(i3917[4], i3917[5], 0, i3916, 'HideMeshRenderer')
  request.r(i3917[6], i3917[7], 0, i3916, 'BoxCollider')
  request.r(i3917[8], i3917[9], 0, i3916, 'MainMaterial')
  request.r(i3917[10], i3917[11], 0, i3916, 'TranparentMaterial')
  request.r(i3917[12], i3917[13], 0, i3916, 'WoolAnimationData')
  var i3919 = i3917[14]
  var i3918 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i3919.length; i += 2) {
  request.r(i3919[i + 0], i3919[i + 1], 1, i3918, '')
  }
  i3916.DecoreControls = i3918
  var i3921 = i3917[15]
  var i3920 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i3921.length; i += 2) {
  request.r(i3921[i + 0], i3921[i + 1], 1, i3920, '')
  }
  i3916.RemovedDecoreControls = i3920
  request.r(i3917[16], i3917[17], 0, i3916, 'MeshFilter')
  i3916.IsSetColorHightest = !!i3917[18]
  var i3923 = i3917[19]
  var i3922 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Vector3')))
  for(var i = 0; i < i3923.length; i += 3) {
    i3922.add(new pc.Vec3( i3923[i + 0], i3923[i + 1], i3923[i + 2] ));
  }
  i3916._spiralPath = i3922
  var i3925 = i3917[20]
  var i3924 = new (System.Collections.Generic.List$1(Bridge.ns('System.Single')))
  for(var i = 0; i < i3925.length; i += 1) {
    i3924.add(i3925[i + 0]);
  }
  i3916._spiralPathUVY = i3924
  return i3916
}

Deserializers["MeshObjectData"] = function (request, data, root) {
  var i3926 = root || request.c( 'MeshObjectData' )
  var i3927 = data
  i3926.TotalLayer = i3927[0]
  i3926.HightestColor = new pc.Color(i3927[1], i3927[2], i3927[3], i3927[4])
  var i3929 = i3927[5]
  var i3928 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i3929.length; i += 4) {
    i3928.add(new pc.Color(i3929[i + 0], i3929[i + 1], i3929[i + 2], i3929[i + 3]));
  }
  i3926.ColorStack = i3928
  return i3926
}

Deserializers["DecoreControl"] = function (request, data, root) {
  var i3936 = root || request.c( 'DecoreControl' )
  var i3937 = data
  request.r(i3937[0], i3937[1], 0, i3936, 'Rigid')
  request.r(i3937[2], i3937[3], 0, i3936, 'MeshRenderer')
  i3936._color = new pc.Color(i3937[4], i3937[5], i3937[6], i3937[7])
  i3936.WoolProgressStartDrop = i3937[8]
  i3936._decoreState = i3937[9]
  return i3936
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Rigidbody"] = function (request, data, root) {
  var i3938 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Rigidbody' )
  var i3939 = data
  i3938.mass = i3939[0]
  i3938.drag = i3939[1]
  i3938.angularDrag = i3939[2]
  i3938.useGravity = !!i3939[3]
  i3938.isKinematic = !!i3939[4]
  i3938.constraints = i3939[5]
  i3938.maxAngularVelocity = i3939[6]
  i3938.collisionDetectionMode = i3939[7]
  i3938.interpolation = i3939[8]
  return i3938
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i3940 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i3941 = data
  i3940.enabled = !!i3941[0]
  i3940.aspect = i3941[1]
  i3940.orthographic = !!i3941[2]
  i3940.orthographicSize = i3941[3]
  i3940.backgroundColor = new pc.Color(i3941[4], i3941[5], i3941[6], i3941[7])
  i3940.nearClipPlane = i3941[8]
  i3940.farClipPlane = i3941[9]
  i3940.fieldOfView = i3941[10]
  i3940.depth = i3941[11]
  i3940.clearFlags = i3941[12]
  i3940.cullingMask = i3941[13]
  i3940.rect = i3941[14]
  request.r(i3941[15], i3941[16], 0, i3940, 'targetTexture')
  i3940.usePhysicalProperties = !!i3941[17]
  i3940.focalLength = i3941[18]
  i3940.sensorSize = new pc.Vec2( i3941[19], i3941[20] )
  i3940.lensShift = new pc.Vec2( i3941[21], i3941[22] )
  i3940.gateFit = i3941[23]
  i3940.commandBufferCount = i3941[24]
  i3940.cameraType = i3941[25]
  return i3940
}

Deserializers["CameraContainer"] = function (request, data, root) {
  var i3942 = root || request.c( 'CameraContainer' )
  var i3943 = data
  request.r(i3943[0], i3943[1], 0, i3942, 'MainCamera')
  request.r(i3943[2], i3943[3], 0, i3942, 'FakeUICamera')
  request.r(i3943[4], i3943[5], 0, i3942, 'EndgameModelCamera')
  return i3942
}

Deserializers["UnityEngine.EventSystems.EventSystem"] = function (request, data, root) {
  var i3944 = root || request.c( 'UnityEngine.EventSystems.EventSystem' )
  var i3945 = data
  request.r(i3945[0], i3945[1], 0, i3944, 'm_FirstSelected')
  i3944.m_sendNavigationEvents = !!i3945[2]
  i3944.m_DragThreshold = i3945[3]
  return i3944
}

Deserializers["UnityEngine.EventSystems.StandaloneInputModule"] = function (request, data, root) {
  var i3946 = root || request.c( 'UnityEngine.EventSystems.StandaloneInputModule' )
  var i3947 = data
  i3946.m_HorizontalAxis = i3947[0]
  i3946.m_VerticalAxis = i3947[1]
  i3946.m_SubmitButton = i3947[2]
  i3946.m_CancelButton = i3947[3]
  i3946.m_InputActionsPerSecond = i3947[4]
  i3946.m_RepeatDelay = i3947[5]
  i3946.m_ForceModuleActive = !!i3947[6]
  i3946.m_SendPointerHoverToParent = !!i3947[7]
  return i3946
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i3948 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i3949 = data
  i3948.ambientIntensity = i3949[0]
  i3948.reflectionIntensity = i3949[1]
  i3948.ambientMode = i3949[2]
  i3948.ambientLight = new pc.Color(i3949[3], i3949[4], i3949[5], i3949[6])
  i3948.ambientSkyColor = new pc.Color(i3949[7], i3949[8], i3949[9], i3949[10])
  i3948.ambientGroundColor = new pc.Color(i3949[11], i3949[12], i3949[13], i3949[14])
  i3948.ambientEquatorColor = new pc.Color(i3949[15], i3949[16], i3949[17], i3949[18])
  i3948.fogColor = new pc.Color(i3949[19], i3949[20], i3949[21], i3949[22])
  i3948.fogEndDistance = i3949[23]
  i3948.fogStartDistance = i3949[24]
  i3948.fogDensity = i3949[25]
  i3948.fog = !!i3949[26]
  request.r(i3949[27], i3949[28], 0, i3948, 'skybox')
  i3948.fogMode = i3949[29]
  var i3951 = i3949[30]
  var i3950 = []
  for(var i = 0; i < i3951.length; i += 1) {
    i3950.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i3951[i + 0]) );
  }
  i3948.lightmaps = i3950
  i3948.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i3949[31], i3948.lightProbes)
  i3948.lightmapsMode = i3949[32]
  i3948.mixedBakeMode = i3949[33]
  i3948.environmentLightingMode = i3949[34]
  i3948.ambientProbe = new pc.SphericalHarmonicsL2(i3949[35])
  i3948.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i3949[36])
  i3948.useReferenceAmbientProbe = !!i3949[37]
  request.r(i3949[38], i3949[39], 0, i3948, 'customReflection')
  request.r(i3949[40], i3949[41], 0, i3948, 'defaultReflection')
  i3948.defaultReflectionMode = i3949[42]
  i3948.defaultReflectionResolution = i3949[43]
  i3948.sunLightObjectId = i3949[44]
  i3948.pixelLightCount = i3949[45]
  i3948.defaultReflectionHDR = !!i3949[46]
  i3948.hasLightDataAsset = !!i3949[47]
  i3948.hasManualGenerate = !!i3949[48]
  return i3948
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i3954 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i3955 = data
  request.r(i3955[0], i3955[1], 0, i3954, 'lightmapColor')
  request.r(i3955[2], i3955[3], 0, i3954, 'lightmapDirection')
  return i3954
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i3956 = root || new UnityEngine.LightProbes()
  var i3957 = data
  return i3956
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i3962 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i3963 = data
  var i3965 = i3963[0]
  var i3964 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i3965.length; i += 1) {
    i3964.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i3965[i + 0]));
  }
  i3962.ShaderCompilationErrors = i3964
  i3962.name = i3963[1]
  i3962.guid = i3963[2]
  var i3967 = i3963[3]
  var i3966 = []
  for(var i = 0; i < i3967.length; i += 1) {
    i3966.push( i3967[i + 0] );
  }
  i3962.shaderDefinedKeywords = i3966
  var i3969 = i3963[4]
  var i3968 = []
  for(var i = 0; i < i3969.length; i += 1) {
    i3968.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i3969[i + 0]) );
  }
  i3962.passes = i3968
  var i3971 = i3963[5]
  var i3970 = []
  for(var i = 0; i < i3971.length; i += 1) {
    i3970.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i3971[i + 0]) );
  }
  i3962.usePasses = i3970
  var i3973 = i3963[6]
  var i3972 = []
  for(var i = 0; i < i3973.length; i += 1) {
    i3972.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i3973[i + 0]) );
  }
  i3962.defaultParameterValues = i3972
  request.r(i3963[7], i3963[8], 0, i3962, 'unityFallbackShader')
  i3962.readDepth = !!i3963[9]
  i3962.isCreatedByShaderGraph = !!i3963[10]
  i3962.compiled = !!i3963[11]
  return i3962
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i3976 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i3977 = data
  i3976.shaderName = i3977[0]
  i3976.errorMessage = i3977[1]
  return i3976
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i3982 = root || new pc.UnityShaderPass()
  var i3983 = data
  i3982.id = i3983[0]
  i3982.subShaderIndex = i3983[1]
  i3982.name = i3983[2]
  i3982.passType = i3983[3]
  i3982.grabPassTextureName = i3983[4]
  i3982.usePass = !!i3983[5]
  i3982.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[6], i3982.zTest)
  i3982.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[7], i3982.zWrite)
  i3982.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[8], i3982.culling)
  i3982.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i3983[9], i3982.blending)
  i3982.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i3983[10], i3982.alphaBlending)
  i3982.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[11], i3982.colorWriteMask)
  i3982.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[12], i3982.offsetUnits)
  i3982.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[13], i3982.offsetFactor)
  i3982.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[14], i3982.stencilRef)
  i3982.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[15], i3982.stencilReadMask)
  i3982.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3983[16], i3982.stencilWriteMask)
  i3982.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3983[17], i3982.stencilOp)
  i3982.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3983[18], i3982.stencilOpFront)
  i3982.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3983[19], i3982.stencilOpBack)
  var i3985 = i3983[20]
  var i3984 = []
  for(var i = 0; i < i3985.length; i += 1) {
    i3984.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i3985[i + 0]) );
  }
  i3982.tags = i3984
  var i3987 = i3983[21]
  var i3986 = []
  for(var i = 0; i < i3987.length; i += 1) {
    i3986.push( i3987[i + 0] );
  }
  i3982.passDefinedKeywords = i3986
  var i3989 = i3983[22]
  var i3988 = []
  for(var i = 0; i < i3989.length; i += 1) {
    i3988.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i3989[i + 0]) );
  }
  i3982.passDefinedKeywordGroups = i3988
  var i3991 = i3983[23]
  var i3990 = []
  for(var i = 0; i < i3991.length; i += 1) {
    i3990.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i3991[i + 0]) );
  }
  i3982.variants = i3990
  var i3993 = i3983[24]
  var i3992 = []
  for(var i = 0; i < i3993.length; i += 1) {
    i3992.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i3993[i + 0]) );
  }
  i3982.excludedVariants = i3992
  i3982.hasDepthReader = !!i3983[25]
  return i3982
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i3994 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i3995 = data
  i3994.val = i3995[0]
  i3994.name = i3995[1]
  return i3994
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i3996 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i3997 = data
  i3996.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3997[0], i3996.src)
  i3996.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3997[1], i3996.dst)
  i3996.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3997[2], i3996.op)
  return i3996
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i3998 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i3999 = data
  i3998.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3999[0], i3998.pass)
  i3998.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3999[1], i3998.fail)
  i3998.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3999[2], i3998.zFail)
  i3998.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3999[3], i3998.comp)
  return i3998
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i4002 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i4003 = data
  i4002.name = i4003[0]
  i4002.value = i4003[1]
  return i4002
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i4006 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i4007 = data
  var i4009 = i4007[0]
  var i4008 = []
  for(var i = 0; i < i4009.length; i += 1) {
    i4008.push( i4009[i + 0] );
  }
  i4006.keywords = i4008
  i4006.hasDiscard = !!i4007[1]
  return i4006
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i4012 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i4013 = data
  i4012.passId = i4013[0]
  i4012.subShaderIndex = i4013[1]
  var i4015 = i4013[2]
  var i4014 = []
  for(var i = 0; i < i4015.length; i += 1) {
    i4014.push( i4015[i + 0] );
  }
  i4012.keywords = i4014
  i4012.vertexProgram = i4013[3]
  i4012.fragmentProgram = i4013[4]
  i4012.exportedForWebGl2 = !!i4013[5]
  i4012.readDepth = !!i4013[6]
  return i4012
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i4018 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i4019 = data
  request.r(i4019[0], i4019[1], 0, i4018, 'shader')
  i4018.pass = i4019[2]
  return i4018
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i4022 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i4023 = data
  i4022.name = i4023[0]
  i4022.type = i4023[1]
  i4022.value = new pc.Vec4( i4023[2], i4023[3], i4023[4], i4023[5] )
  i4022.textureValue = i4023[6]
  i4022.shaderPropertyFlag = i4023[7]
  return i4022
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i4024 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i4025 = data
  i4024.name = i4025[0]
  request.r(i4025[1], i4025[2], 0, i4024, 'texture')
  i4024.aabb = i4025[3]
  i4024.vertices = i4025[4]
  i4024.triangles = i4025[5]
  i4024.textureRect = UnityEngine.Rect.MinMaxRect(i4025[6], i4025[7], i4025[8], i4025[9])
  i4024.packedRect = UnityEngine.Rect.MinMaxRect(i4025[10], i4025[11], i4025[12], i4025[13])
  i4024.border = new pc.Vec4( i4025[14], i4025[15], i4025[16], i4025[17] )
  i4024.transparency = i4025[18]
  i4024.bounds = i4025[19]
  i4024.pixelsPerUnit = i4025[20]
  i4024.textureWidth = i4025[21]
  i4024.textureHeight = i4025[22]
  i4024.nativeSize = new pc.Vec2( i4025[23], i4025[24] )
  i4024.pivot = new pc.Vec2( i4025[25], i4025[26] )
  i4024.textureRectOffset = new pc.Vec2( i4025[27], i4025[28] )
  return i4024
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.AudioClip"] = function (request, data, root) {
  var i4026 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.AudioClip' )
  var i4027 = data
  i4026.name = i4027[0]
  return i4026
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font"] = function (request, data, root) {
  var i4028 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font' )
  var i4029 = data
  i4028.name = i4029[0]
  i4028.ascent = i4029[1]
  i4028.originalLineHeight = i4029[2]
  i4028.fontSize = i4029[3]
  var i4031 = i4029[4]
  var i4030 = []
  for(var i = 0; i < i4031.length; i += 1) {
    i4030.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo', i4031[i + 0]) );
  }
  i4028.characterInfo = i4030
  request.r(i4029[5], i4029[6], 0, i4028, 'texture')
  i4028.originalFontSize = i4029[7]
  return i4028
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo"] = function (request, data, root) {
  var i4034 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo' )
  var i4035 = data
  i4034.index = i4035[0]
  i4034.advance = i4035[1]
  i4034.bearing = i4035[2]
  i4034.glyphWidth = i4035[3]
  i4034.glyphHeight = i4035[4]
  i4034.minX = i4035[5]
  i4034.maxX = i4035[6]
  i4034.minY = i4035[7]
  i4034.maxY = i4035[8]
  i4034.uvBottomLeftX = i4035[9]
  i4034.uvBottomLeftY = i4035[10]
  i4034.uvBottomRightX = i4035[11]
  i4034.uvBottomRightY = i4035[12]
  i4034.uvTopLeftX = i4035[13]
  i4034.uvTopLeftY = i4035[14]
  i4034.uvTopRightX = i4035[15]
  i4034.uvTopRightY = i4035[16]
  return i4034
}

Deserializers["WoolAnimationData"] = function (request, data, root) {
  var i4036 = root || request.c( 'WoolAnimationData' )
  var i4037 = data
  i4036.Duration = i4037[0]
  i4036.DurationHideWool = i4037[1]
  i4036.OffSet = i4037[2]
  i4036.ForceValue = i4037[3]
  i4036.RandomDirrectionFactor = i4037[4]
  return i4036
}

Deserializers["ZoomCameraData"] = function (request, data, root) {
  var i4038 = root || request.c( 'ZoomCameraData' )
  var i4039 = data
  i4038.ZoomSpeed = i4039[0]
  i4038.MinFOV = i4039[1]
  i4038.MaxFOV = i4039[2]
  i4038.DefaultFOV = i4039[3]
  return i4038
}

Deserializers["DG.Tweening.Core.DOTweenSettings"] = function (request, data, root) {
  var i4040 = root || request.c( 'DG.Tweening.Core.DOTweenSettings' )
  var i4041 = data
  i4040.useSafeMode = !!i4041[0]
  i4040.safeModeOptions = request.d('DG.Tweening.Core.DOTweenSettings+SafeModeOptions', i4041[1], i4040.safeModeOptions)
  i4040.timeScale = i4041[2]
  i4040.unscaledTimeScale = i4041[3]
  i4040.useSmoothDeltaTime = !!i4041[4]
  i4040.maxSmoothUnscaledTime = i4041[5]
  i4040.rewindCallbackMode = i4041[6]
  i4040.showUnityEditorReport = !!i4041[7]
  i4040.logBehaviour = i4041[8]
  i4040.drawGizmos = !!i4041[9]
  i4040.defaultRecyclable = !!i4041[10]
  i4040.defaultAutoPlay = i4041[11]
  i4040.defaultUpdateType = i4041[12]
  i4040.defaultTimeScaleIndependent = !!i4041[13]
  i4040.defaultEaseType = i4041[14]
  i4040.defaultEaseOvershootOrAmplitude = i4041[15]
  i4040.defaultEasePeriod = i4041[16]
  i4040.defaultAutoKill = !!i4041[17]
  i4040.defaultLoopType = i4041[18]
  i4040.debugMode = !!i4041[19]
  i4040.debugStoreTargetId = !!i4041[20]
  i4040.showPreviewPanel = !!i4041[21]
  i4040.storeSettingsLocation = i4041[22]
  i4040.modules = request.d('DG.Tweening.Core.DOTweenSettings+ModulesSetup', i4041[23], i4040.modules)
  i4040.createASMDEF = !!i4041[24]
  i4040.showPlayingTweens = !!i4041[25]
  i4040.showPausedTweens = !!i4041[26]
  return i4040
}

Deserializers["DG.Tweening.Core.DOTweenSettings+SafeModeOptions"] = function (request, data, root) {
  var i4042 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+SafeModeOptions' )
  var i4043 = data
  i4042.logBehaviour = i4043[0]
  i4042.nestedTweenFailureBehaviour = i4043[1]
  return i4042
}

Deserializers["DG.Tweening.Core.DOTweenSettings+ModulesSetup"] = function (request, data, root) {
  var i4044 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+ModulesSetup' )
  var i4045 = data
  i4044.showPanel = !!i4045[0]
  i4044.audioEnabled = !!i4045[1]
  i4044.physicsEnabled = !!i4045[2]
  i4044.physics2DEnabled = !!i4045[3]
  i4044.spriteEnabled = !!i4045[4]
  i4044.uiEnabled = !!i4045[5]
  i4044.textMeshProEnabled = !!i4045[6]
  i4044.tk2DEnabled = !!i4045[7]
  i4044.deAudioEnabled = !!i4045[8]
  i4044.deUnityExtendedEnabled = !!i4045[9]
  i4044.epoOutlineEnabled = !!i4045[10]
  return i4044
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i4046 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i4047 = data
  var i4049 = i4047[0]
  var i4048 = []
  for(var i = 0; i < i4049.length; i += 1) {
    i4048.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i4049[i + 0]) );
  }
  i4046.files = i4048
  i4046.componentToPrefabIds = i4047[1]
  return i4046
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i4052 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i4053 = data
  i4052.path = i4053[0]
  request.r(i4053[1], i4053[2], 0, i4052, 'unityObject')
  return i4052
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i4054 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i4055 = data
  var i4057 = i4055[0]
  var i4056 = []
  for(var i = 0; i < i4057.length; i += 1) {
    i4056.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i4057[i + 0]) );
  }
  i4054.scriptsExecutionOrder = i4056
  var i4059 = i4055[1]
  var i4058 = []
  for(var i = 0; i < i4059.length; i += 1) {
    i4058.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i4059[i + 0]) );
  }
  i4054.sortingLayers = i4058
  var i4061 = i4055[2]
  var i4060 = []
  for(var i = 0; i < i4061.length; i += 1) {
    i4060.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i4061[i + 0]) );
  }
  i4054.cullingLayers = i4060
  i4054.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i4055[3], i4054.timeSettings)
  i4054.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i4055[4], i4054.physicsSettings)
  i4054.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i4055[5], i4054.physics2DSettings)
  i4054.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i4055[6], i4054.qualitySettings)
  i4054.enableRealtimeShadows = !!i4055[7]
  i4054.enableAutoInstancing = !!i4055[8]
  i4054.enableDynamicBatching = !!i4055[9]
  i4054.lightmapEncodingQuality = i4055[10]
  i4054.desiredColorSpace = i4055[11]
  var i4063 = i4055[12]
  var i4062 = []
  for(var i = 0; i < i4063.length; i += 1) {
    i4062.push( i4063[i + 0] );
  }
  i4054.allTags = i4062
  return i4054
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i4066 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i4067 = data
  i4066.name = i4067[0]
  i4066.value = i4067[1]
  return i4066
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i4070 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i4071 = data
  i4070.id = i4071[0]
  i4070.name = i4071[1]
  i4070.value = i4071[2]
  return i4070
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i4074 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i4075 = data
  i4074.id = i4075[0]
  i4074.name = i4075[1]
  return i4074
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i4076 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i4077 = data
  i4076.fixedDeltaTime = i4077[0]
  i4076.maximumDeltaTime = i4077[1]
  i4076.timeScale = i4077[2]
  i4076.maximumParticleTimestep = i4077[3]
  return i4076
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i4078 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i4079 = data
  i4078.gravity = new pc.Vec3( i4079[0], i4079[1], i4079[2] )
  i4078.defaultSolverIterations = i4079[3]
  i4078.bounceThreshold = i4079[4]
  i4078.autoSyncTransforms = !!i4079[5]
  i4078.autoSimulation = !!i4079[6]
  var i4081 = i4079[7]
  var i4080 = []
  for(var i = 0; i < i4081.length; i += 1) {
    i4080.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i4081[i + 0]) );
  }
  i4078.collisionMatrix = i4080
  return i4078
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i4084 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i4085 = data
  i4084.enabled = !!i4085[0]
  i4084.layerId = i4085[1]
  i4084.otherLayerId = i4085[2]
  return i4084
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i4086 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i4087 = data
  request.r(i4087[0], i4087[1], 0, i4086, 'material')
  i4086.gravity = new pc.Vec2( i4087[2], i4087[3] )
  i4086.positionIterations = i4087[4]
  i4086.velocityIterations = i4087[5]
  i4086.velocityThreshold = i4087[6]
  i4086.maxLinearCorrection = i4087[7]
  i4086.maxAngularCorrection = i4087[8]
  i4086.maxTranslationSpeed = i4087[9]
  i4086.maxRotationSpeed = i4087[10]
  i4086.baumgarteScale = i4087[11]
  i4086.baumgarteTOIScale = i4087[12]
  i4086.timeToSleep = i4087[13]
  i4086.linearSleepTolerance = i4087[14]
  i4086.angularSleepTolerance = i4087[15]
  i4086.defaultContactOffset = i4087[16]
  i4086.autoSimulation = !!i4087[17]
  i4086.queriesHitTriggers = !!i4087[18]
  i4086.queriesStartInColliders = !!i4087[19]
  i4086.callbacksOnDisable = !!i4087[20]
  i4086.reuseCollisionCallbacks = !!i4087[21]
  i4086.autoSyncTransforms = !!i4087[22]
  var i4089 = i4087[23]
  var i4088 = []
  for(var i = 0; i < i4089.length; i += 1) {
    i4088.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i4089[i + 0]) );
  }
  i4086.collisionMatrix = i4088
  return i4086
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i4092 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i4093 = data
  i4092.enabled = !!i4093[0]
  i4092.layerId = i4093[1]
  i4092.otherLayerId = i4093[2]
  return i4092
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i4094 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i4095 = data
  var i4097 = i4095[0]
  var i4096 = []
  for(var i = 0; i < i4097.length; i += 1) {
    i4096.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i4097[i + 0]) );
  }
  i4094.qualityLevels = i4096
  var i4099 = i4095[1]
  var i4098 = []
  for(var i = 0; i < i4099.length; i += 1) {
    i4098.push( i4099[i + 0] );
  }
  i4094.names = i4098
  i4094.shadows = i4095[2]
  i4094.anisotropicFiltering = i4095[3]
  i4094.antiAliasing = i4095[4]
  i4094.lodBias = i4095[5]
  i4094.shadowCascades = i4095[6]
  i4094.shadowDistance = i4095[7]
  i4094.shadowmaskMode = i4095[8]
  i4094.shadowProjection = i4095[9]
  i4094.shadowResolution = i4095[10]
  i4094.softParticles = !!i4095[11]
  i4094.softVegetation = !!i4095[12]
  i4094.activeColorSpace = i4095[13]
  i4094.desiredColorSpace = i4095[14]
  i4094.masterTextureLimit = i4095[15]
  i4094.maxQueuedFrames = i4095[16]
  i4094.particleRaycastBudget = i4095[17]
  i4094.pixelLightCount = i4095[18]
  i4094.realtimeReflectionProbes = !!i4095[19]
  i4094.shadowCascade2Split = i4095[20]
  i4094.shadowCascade4Split = new pc.Vec3( i4095[21], i4095[22], i4095[23] )
  i4094.streamingMipmapsActive = !!i4095[24]
  i4094.vSyncCount = i4095[25]
  i4094.asyncUploadBufferSize = i4095[26]
  i4094.asyncUploadTimeSlice = i4095[27]
  i4094.billboardsFaceCameraPosition = !!i4095[28]
  i4094.shadowNearPlaneOffset = i4095[29]
  i4094.streamingMipmapsMemoryBudget = i4095[30]
  i4094.maximumLODLevel = i4095[31]
  i4094.streamingMipmapsAddAllCameras = !!i4095[32]
  i4094.streamingMipmapsMaxLevelReduction = i4095[33]
  i4094.streamingMipmapsRenderersPerFrame = i4095[34]
  i4094.resolutionScalingFixedDPIFactor = i4095[35]
  i4094.streamingMipmapsMaxFileIORequests = i4095[36]
  i4094.currentQualityLevel = i4095[37]
  return i4094
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixer"] = function (request, data, root) {
  var i4102 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixer' )
  var i4103 = data
  var i4105 = i4103[0]
  var i4104 = []
  for(var i = 0; i < i4105.length; i += 1) {
    i4104.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup', i4105[i + 0]) );
  }
  i4102.groups = i4104
  var i4107 = i4103[1]
  var i4106 = []
  for(var i = 0; i < i4107.length; i += 1) {
    i4106.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot', i4107[i + 0]) );
  }
  i4102.snapshots = i4106
  return i4102
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup"] = function (request, data, root) {
  var i4110 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup' )
  var i4111 = data
  i4110.id = i4111[0]
  i4110.childGroupIds = i4111[1]
  i4110.name = i4111[2]
  return i4110
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot"] = function (request, data, root) {
  var i4114 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot' )
  var i4115 = data
  i4114.id = i4115[0]
  var i4117 = i4115[1]
  var i4116 = []
  for(var i = 0; i < i4117.length; i += 1) {
    i4116.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter', i4117[i + 0]) );
  }
  i4114.parameters = i4116
  return i4114
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter"] = function (request, data, root) {
  var i4120 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter' )
  var i4121 = data
  i4120.name = i4121[0]
  i4120.value = i4121[1]
  return i4120
}

Deserializers["UnityEngine.Events.ArgumentCache"] = function (request, data, root) {
  var i4122 = root || request.c( 'UnityEngine.Events.ArgumentCache' )
  var i4123 = data
  request.r(i4123[0], i4123[1], 0, i4122, 'm_ObjectArgument')
  i4122.m_ObjectArgumentAssemblyTypeName = i4123[2]
  i4122.m_IntArgument = i4123[3]
  i4122.m_FloatArgument = i4123[4]
  i4122.m_StringArgument = i4123[5]
  i4122.m_BoolArgument = !!i4123[6]
  return i4122
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame"] = function (request, data, root) {
  var i4126 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame' )
  var i4127 = data
  i4126.weight = i4127[0]
  i4126.vertices = i4127[1]
  i4126.normals = i4127[2]
  i4126.tangents = i4127[3]
  return i4126
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

Deserializers.buildID = "84bd5206-f89f-48ff-b686-bc33cd5ac933";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

