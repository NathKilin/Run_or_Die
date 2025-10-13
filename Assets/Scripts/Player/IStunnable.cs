/*
 * IStunnable
 * ----------
 * Tiny interface so hazards can tell the player to enter a "stunned" state.
 * Keeping it in the global namespace avoids namespace-mismatch errors.
 */
public interface IStunnable
{
    void Stun();
}
