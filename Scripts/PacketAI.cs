using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

namespace Pathfinding
{
	[RequireComponent(typeof(Seeker))]
	public class PacketAI : AIPath
	{
		public float sleepVelocity = 0.4F; // Minimum velocity for moving
		public GameObject endOfPathEffect; // Effect which will be instantiated when end of path is reached.

		protected Vector3 lastTarget; // Point for the last spawn of #endOfPathEffect

		public new void Start()
		{
			base.Start(); // Call Start in base script (AIPath)
		}

		protected new void Update()
		{
			//Get velocity in world-space
			//Vector3 velocity;
			if (canMove)
			{

				//Calculate desired velocity
				Vector3 dir = CalculateVelocity(GetFeetPosition());

				//Rotate towards targetDirection (filled in by CalculateVelocity)
				RotateTowards(targetDirection);

				dir.y = 0;
				if (dir.sqrMagnitude > sleepVelocity * sleepVelocity)
				{
					//If the velocity is large enough, move
				}
				else
				{
					//Otherwise, just stand still (this ensures gravity is applied)
					dir = Vector3.zero;
				}

				if (this.rvoController != null)
				{
					rvoController.Move(dir);
					//velocity = rvoController.velocity;
				}
				else
					if (navController != null)
					{
#if FALSE
					navController.SimpleMove (GetFeetPosition(), dir);
#endif
						//velocity = Vector3.zero;
					}
					else if (controller != null)
					{
						controller.SimpleMove(dir);
						//velocity = controller.velocity;
					}
					else
					{
						Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
						//velocity = Vector3.zero;
					}
			}
			else
			{
				//velocity = Vector3.zero;
			}
		}

		// Called when the end of path has been reached to instntiate an effect 'endOfPathEffect'
		public override void OnTargetReached()
		{
			if (endOfPathEffect != null && Vector3.Distance(tr.position, lastTarget) > 1)
			{
				GameObject.Instantiate(endOfPathEffect, tr.position, tr.rotation);
				lastTarget = tr.position;
			}
		}

		// for desired velocity
		public override Vector3 GetFeetPosition()
		{
			return tr.position;
		}
	}
}
