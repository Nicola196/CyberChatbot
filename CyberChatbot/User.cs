

namespace Cyber_securityChatbot
{

    /// The User class holds information about the person
    /// currently chatting with the bot. It uses automatic

    public class User
    {

        // PROPERTY: Name
        // Stores the user's name so the bot can personalise replies.
        // "{ get; set; }" means it can be read AND changed anywhere.
        // ---------------------------------------------------------
        public string Name { get; set; }

        // ---------------------------------------------------------
        // PROPERTY: IsExiting
        // A true/false flag. When the user types "exit", this
        // is set to true so the main chat loop knows to stop.
        // ---------------------------------------------------------
        public bool IsExiting { get; set; }

        // ---------------------------------------------------------
        // CONSTRUCTOR
        // Called when we create a new User object.
        // Sets a default name and ensures IsExiting starts as false.
        // ---------------------------------------------------------
        public User(string name)
        {
            Name = name;        // Save the name passed in
            IsExiting = false;  // The user has NOT asked to exit yet
        }
    }
}