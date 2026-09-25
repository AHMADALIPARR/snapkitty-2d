// SNAPKITTY: Bifrost Runner — player character (the hoodie kitten).
// UnrealSharp C# — attach flipbooks imported from art/kitten.png in the editor.

using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;
using UnrealSharp.InputCore;
using UnrealSharp.Paper2D;

namespace Snapkitty2D;

[UClass]
public class ASnapkittyPlayer : ACharacter
{
    // ---- tuning (exposed to Blueprints) ----
    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Movement")]
    public float RunSpeed { get; set; } = 420.0f;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Movement")]
    public float JumpHeight { get; set; } = 620.0f;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Animation")]
    public UPaperFlipbook? IdleFlipbook { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Animation")]
    public UPaperFlipbook? RunFlipbook { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Animation")]
    public UPaperFlipbook? JumpFlipbook { get; set; }

    // ---- input ----
    [UProperty(PropertyFlags.EditAnywhere, Category = "Input")]
    public UInputAction? MoveAction { get; set; }

    [UProperty(PropertyFlags.EditAnywhere, Category = "Input")]
    public UInputAction? JumpAction { get; set; }

    [UProperty(PropertyFlags.EditAnywhere, Category = "Input")]
    public UInputMappingContext? MappingContext { get; set; }

    private UPaperFlipbookComponent? sprite;
    private float moveAxis;

    public ASnapkittyPlayer()
    {
        // 2D side view: lock the character to the X/Z plane.
        var movement = CharacterMovement;
        movement.bConstrainToPlane = true;
        movement.SetPlaneConstraintNormal(new FVector(0, 1, 0));
        movement.MaxWalkSpeed = RunSpeed;
        movement.JumpZVelocity = JumpHeight;
        movement.AirControl = 0.8f;
    }

    public override void BeginPlay()
    {
        base.BeginPlay();
        sprite = GetComponentByClass<UPaperFlipbookComponent>();
        if (APlayerController.PlayerControllers.Count > 0 &&
            APlayerController.PlayerControllers[0] is APlayerController pc)
        {
            var subsystem = USubsystemBlueprintLibrary.GetLocalPlayerSubSystem<UEnhancedInputLocalPlayerSubsystem>(pc);
            subsystem?.AddMappingContext(MappingContext, 0);
        }
    }

    public override void SetupPlayerInputComponent(UInputComponent playerInputComponent)
    {
        base.SetupPlayerInputComponent(playerInputComponent);
        if (playerInputComponent is UEnhancedInputComponent input)
        {
            input.BindAction(MoveAction, ETriggerEvent.Triggered, this, nameof(OnMove));
            input.BindAction(MoveAction, ETriggerEvent.Completed, this, nameof(OnMoveReleased));
            input.BindAction(JumpAction, ETriggerEvent.Started, this, nameof(OnJumpPressed));
        }
    }

    [UFunction]
    public void OnMove(FInputActionValue value)
    {
        moveAxis = value.Get<float>();
    }

    [UFunction]
    public void OnMoveReleased(FInputActionValue value)
    {
        moveAxis = 0.0f;
    }

    [UFunction]
    public void OnJumpPressed(FInputActionValue value)
    {
        Jump();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Math.Abs(moveAxis) > 0.01f)
        {
            AddMovementInput(new FVector(moveAxis, 0, 0), 1.0f);
            // face travel direction
            var rot = K2_GetActorRotation();
            rot.Yaw = moveAxis > 0 ? 0.0f : 180.0f;
            K2_SetActorRotation(rot, false);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (sprite is null) return;
        var movement = CharacterMovement;
        UPaperFlipbook? want = IdleFlipbook;
        if (movement.IsFalling()) want = JumpFlipbook;
        else if (Math.Abs(movement.Velocity.X) > 10.0f) want = RunFlipbook;
        if (want is not null && sprite.GetFlipbook() != want)
            sprite.SetFlipbook(want);
    }

    // Called by BifrostPickup when the kitten seals an event cube.
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Bifrost")]
    public void OnEventSealed(int totalSealed)
    {
        var gm = UGameplayStatics.GetGameMode(this) as ASnapkittyGameMode;
        gm?.NotifyEventSealed(totalSealed);
    }
}
