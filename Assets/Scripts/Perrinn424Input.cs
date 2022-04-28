

using UnityEngine;
using VehiclePhysics;
using VehiclePhysics.InputManagement;
using EdyCommonTools;


namespace Perrinn424
{

class Perrinn424InputUser : InputUser
	{
	public Perrinn424InputUser (string idName) : base(idName) { }

	public InputAxis steer 			= new InputAxis("Steer");
	public InputSlider throttle		= new InputSlider("Throttle");
	public InputSlider brake 		= new InputSlider("Brake");
	public InputButton gearUp		= new InputButton("GearShiftUp");
	public InputButton gearDown		= new InputButton("GearShiftDown");
	public InputButton drsEnable	= new InputButton("DrsEnable");
	}


public class Perrinn424Input : VehicleBehaviour
	{
	public string inputUserName = "No name";

	[Header("Force Feedback")]
	public bool forceFeedbackEnabled = true;
	public bool rumbleEnabled = true;

	[Space(5)]
	public ForceFeedbackHelper.Settings forceFeedbackSettings = new ForceFeedbackHelper.Settings();
	public ForceFeedbackHelper.RumbleSettings rumbleSettings = new ForceFeedbackHelper.RumbleSettings();


	// Private fields

	Perrinn424InputUser m_input;
	ForceFeedbackHelper m_ffHelper;
	bool m_ffEnabled = false;

	Steering.Settings m_steeringSettings;


	// Component implementation


	public override void OnEnableVehicle ()
		{
		m_input = new Perrinn424InputUser(inputUserName);
		InputManager.instance.RegisterUser(m_input);

		m_steeringSettings = vehicle.GetInternalObject(typeof(Steering.Settings)) as Steering.Settings;
		}


	public override void OnDisableVehicle ()
		{
		// Stop force feedback

		InputDevice.ForceFeedback forceFeedback = m_input.steer.ForceFeedback();
		if (m_ffEnabled && forceFeedback != null)
			forceFeedback.StopAllEffects();

		m_ffEnabled = false;

		// Unregister user

		InputManager.instance.UnregisterUser(m_input);
		}


	public override void FixedUpdateVehicle ()
		{
		int[] inputData = vehicle.data.Get(Channel.Input);

		// Throttle, brake

		inputData[InputData.Throttle] = (int)(m_input.throttle.Value() * 10000);
		inputData[InputData.Brake] = (int)(m_input.brake.Value() * 10000);

		// Apply steer based in the physical and logical steering wheel ranges

		float steerInput = m_input.steer.Value();
		float physicalWheelRange = InputManager.instance.settings.physicalWheelRange;

		if (m_steeringSettings != null && m_steeringSettings.steeringWheelRange > 0.0f && physicalWheelRange > 0.0f)
			steerInput = Mathf.Clamp(steerInput / m_steeringSettings.steeringWheelRange * physicalWheelRange, -1.0f, 1.0f);

		inputData[InputData.Steer] = (int)(steerInput * 10000);

		// Gear mode

		if (m_input.gearUp.PressedThisFrame()) inputData[InputData.AutomaticGear]++;
		if (m_input.gearDown.PressedThisFrame()) inputData[InputData.AutomaticGear]--;

		// TODO DRS

		// Process force feedback if available

		InputDevice.ForceFeedback forceFeedback = m_input.steer.ForceFeedback();

		if (m_ffEnabled != forceFeedbackEnabled)
			{
			if (!forceFeedbackEnabled && forceFeedback != null)
				forceFeedback.StopAllEffects();

			if (forceFeedbackEnabled)
				{
				m_ffHelper = new ForceFeedbackHelper(vehicle);
				m_ffHelper.settings = forceFeedbackSettings;
				m_ffHelper.rumbleSettings = rumbleSettings;
				}

			m_ffEnabled = forceFeedbackEnabled;
			}

		if (m_ffEnabled && forceFeedback != null)
			{
			m_ffHelper.Update();
			ProcessForceFeedback(forceFeedback);
			}
		}


	void ProcessForceFeedback (InputDevice.ForceFeedback forceFeedback)
		{
		(float forceFactor, float dragFactor, float frictionFactor) = m_ffHelper.GetForceFeedback();

		// Main force force feedback
		// The direction of a force is the direction from which it comes.
		// A positive force on a given axis pushes from the positive toward the negative.

		forceFeedback.force = true;
		forceFeedback.forceMagnitude = -forceFactor;

		// Play damper resistance based on the weight on the steering wheels

		forceFeedback.damper = true;
		forceFeedback.damperCoefficient = dragFactor;

		// Play extra friction at low speeds

		forceFeedback.friction = true;
		forceFeedback.frictionCoefficient = frictionFactor;

		// Apply rumble

		if (rumbleEnabled)
			{
			(float rumbleIntensity, float rumbleFrequency) = m_ffHelper.GetRumble();

			forceFeedback.rumble = true;
			forceFeedback.rumbleMagnitude = rumbleIntensity;
			forceFeedback.rumbleFrequency = rumbleFrequency;
			}
		else
			{
			forceFeedback.rumble = false;
			}
		}
	}

}