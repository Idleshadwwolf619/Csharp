using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MoonBase
{
    public partial class Form1 : Form
    {
        // Constructor for the main form
        public Form1()
        {
            InitializeComponent();
            InitializeLocations();  // Initialize Moonbase locations
            AddDirectionButtons();  // Add navigation buttons to the form
            DisplayLocation();      // Display initial location
            AddExitButton();        // Add exit button to the form
                                    
            SetupMenuStrip();   // Call a method to set up the MenuStrip

        }

        private void SetupMenuStrip()
        {
            // Create a new MenuStrip
            MenuStrip menuStrip = new MenuStrip();

            // Create ToolStripMenuItems for each location
            foreach (Location location in locations)
            {
                ToolStripMenuItem locationMenuItem = new ToolStripMenuItem(location.Name);
                locationMenuItem.Click += (sender, e) => GoToLocation(location);
                menuStrip.Items.Add(locationMenuItem);
            }

            // Add the MenuStrip to the form
            this.Controls.Add(menuStrip);
        }

        private void GoToLocation(Location location)
        {
            int newIndex = Array.IndexOf(locations, location);
            currentLocationIndex = newIndex;
            DisplayLocation();
        }
        // Array to store Moonbase locations
        private Location[] locations;

        // Index to track the current location
        private int currentLocationIndex = 0;

        // Navigation buttons
        private Button buttonNorth, buttonSouth, buttonEast, buttonWest;

        // Moonbase areas (instances of Location)
        private Location commandCenter, livingQuarters, researchLab, greenhouse, storageRoom, observatory, hallway;

        // Enum to represent directions
        public enum Direction
        {
            East,
            West,
            North,
            South
        }

        // Event handler for the form load event
        private void Form1_Load(object sender, EventArgs e)
        { }

        // Define a class to represent a location in the Moonbase
        public class Location
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }

            // Constructor to initialize a location
            public Location(string name, string description, string imagePath)
            {
                Name = name;
                Description = description;
                ImagePath = imagePath;
            }
        }

        // Method to initialize Moonbase locations
        private void InitializeLocations()
        {
            // Initialize Moonbase areas as Location objects
            commandCenter = new Location("Command Center", "This is the central hub of the Moonbase.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location0.jpg");
            livingQuarters = new Location("Living Quarters", "This is where astronauts live and rest.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location1.jpg");
            researchLab = new Location("Research Lab", "Cutting-edge scientific research is conducted here.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location2.jpg");
            greenhouse = new Location("Greenhouse", "Agricultural experiments are carried out in this controlled environment.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location3.jpg");
            storageRoom = new Location("Storage Room", "Supplies and equipment are stored here.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location4.jpg");
            observatory = new Location("Observatory", "Astronomical observations are made from this facility.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location5.jpg");
            hallway = new Location("Hallway", "A connecting passage between different areas of the Moonbase.", "C:\\Users\\idles\\OneDrive\\Desktop\\UAT\\C#\\MoonBase\\Images/location6.jpg");

            // Store Moonbase areas in an array
            locations = new Location[] { commandCenter, livingQuarters, researchLab, greenhouse, storageRoom, observatory, hallway };
        }

        // Method to add navigation buttons dynamically
        private void AddDirectionButtons()
        {
            int buttonSpacing = 10;
            int buttonHeight = 30;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Button button = new Button
                {
                    Name = $"button{direction}",
                    Text = direction.ToString(),
                    Width = 75,
                    Height = buttonHeight,
                    Location = new Point(10, 100 + (buttonHeight + buttonSpacing) * (int)direction),
                };

                button.Click += (sender, e) => MovePlayer(direction);
                this.Controls.Add(button);

                AssignNavigationButtons(button, direction);
            }
        }

        // Method to assign navigation buttons to corresponding variables
        private void AssignNavigationButtons(Button button, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    buttonNorth = button;
                    break;
                case Direction.South:
                    buttonSouth = button;
                    break;
                case Direction.East:
                    buttonEast = button;
                    break;
                case Direction.West:
                    buttonWest = button;
                    break;
            }
        }

        // Method to handle player movement
        private void MovePlayer(Direction direction)
        {
            int newIndex = currentLocationIndex;

            // Logic to handle player movement based on the chosen direction
            switch (direction)
            {
                case Direction.East:
                    // Move to Living Quarters from Command Center
                    if (locations[currentLocationIndex] == commandCenter)
                        newIndex = Array.IndexOf(locations, livingQuarters);
                    // Move to Command Center from Storage
                    else if (locations[currentLocationIndex] == storageRoom)
                        newIndex = Array.IndexOf(locations, commandCenter);
                    break;
                case Direction.West:
                    // Move to Storage Room from Command Center
                    if (locations[currentLocationIndex] == commandCenter)
                        newIndex = Array.IndexOf(locations, storageRoom);
                    // Move to Command Center from Living Quarters
                    else if (locations[currentLocationIndex] == livingQuarters)
                        newIndex = Array.IndexOf(locations, commandCenter);
                    break;
                case Direction.North:
                    // Move from Command Center to Hallway
                    if (locations[currentLocationIndex] == commandCenter)
                        newIndex = Array.IndexOf(locations, hallway);
                    // Move from Hallway to Research Lab
                    else if (locations[currentLocationIndex] == hallway)
                        newIndex = Array.IndexOf(locations, researchLab);
                    // Move from Observatory to Command Center
                    else if (locations[currentLocationIndex] == observatory)
                        newIndex = Array.IndexOf(locations, commandCenter);
                    break;
                case Direction.South:
                    // Move from Research Lab to Hallway to Command Center
                    if (locations[currentLocationIndex] == researchLab)
                        newIndex = Array.IndexOf(locations, hallway);
                    // Move from Hallway to Command Center
                    else if (locations[currentLocationIndex] == hallway)
                        newIndex = Array.IndexOf(locations, commandCenter);
                    // Move from Command Center to Observatory
                    else if (locations[currentLocationIndex] == commandCenter)
                        newIndex = Array.IndexOf(locations, observatory);
                    break;
            }

            // Display the new location
            currentLocationIndex = newIndex;
            DisplayLocation();
        }

        // Method to display information for the current location
        private void DisplayLocation()
        {
            UpdateLabel("labelLocation", $"You are in the {locations[currentLocationIndex].Name}");
            UpdateLabel("labelDescription", locations[currentLocationIndex].Description);
            LoadBackgroundImage(locations[currentLocationIndex].ImagePath);

            // Adjust the vertical position of the description label to the bottom of the form
            int labelDescriptionTop = this.Height - 40 - labelDescription.Height; // Adjust as needed
            UpdateLabelLocation("labelDescription", new Point(10, labelDescriptionTop));

            UpdateNavigationButtons();
        }

        private void UpdateLabelLocation(string labelName, Point location)
        {
            Control[] foundControls = Controls.Find(labelName, true);

            if (foundControls.Length > 0 && foundControls[0] is Label label)
            {
                label.Location = location;
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void commandCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {

        }

        // Method to update navigation buttons based on user's position
        private void UpdateNavigationButtons()
        {
            buttonNorth.Enabled = CanMove(Direction.North);
            buttonSouth.Enabled = CanMove(Direction.South);
            buttonEast.Enabled = CanMove(Direction.East);
            buttonWest.Enabled = CanMove(Direction.West);
        }

        // Method to check if the user can move in a specified direction
        private bool CanMove(Direction direction)
        {
            // Customize this logic based on your requirements
            switch (direction)
            {
                case Direction.North:
                    // Check if moving north is allowed from the current location
                    return true;
                case Direction.South:
                    // Check if moving south is allowed from the current location
                    return true;
                case Direction.East:
                    // Check if moving east is allowed from the current location
                    return true;
                case Direction.West:
                    // Check if moving west is allowed from the current location
                    return true;
                default:
                    return false;
            }
        }

        // Method to load background image based on the current location
        private void LoadBackgroundImage(string imagePath)
        {
            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    // Dispose of the existing background image, if any
                    if (this.BackgroundImage != null)
                    {
                        this.BackgroundImage.Dispose();
                    }

                    // Load the new background image
                    this.BackgroundImage = Image.FromFile(imagePath);
                }
                else
                {
                    // Set a default background color or image if the file is not found
                    this.BackgroundImage = null;  // Set to null for no background image
                    this.BackColor = Color.Gray;  // Set a default background color
                }
            }
            catch (OutOfMemoryException ex)
            {
                // Handle OutOfMemoryException
                Console.WriteLine($"Out of memory: {ex.Message}");
                // Optionally, you can inform the user or take other actions.
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error loading image: {ex.Message}");
                // Optionally, you can inform the user or take other actions.
            }
        }

        // Method to update label text
        private void UpdateLabel(string labelName, string text)
        {
            Control[] foundControls = Controls.Find(labelName, true);

            if (foundControls.Length > 0 && foundControls[0] is Label label)
            {
                label.Text = text;
            }
        }

        // Event handler for the "Look Around" button click
        private void buttonLookAround_Click(object sender, EventArgs e)
        {
            DisplayMessage("labelMessage", "You look around. Everything seems normal.");
        }

        // Method to display a message in a specified label
        private void DisplayMessage(string labelName, string message)
        {
            UpdateLabel(labelName, message);
        }

        // Event handler for the exit button click
        private void buttonExit_Click(object sender, EventArgs e)
        {
            // Perform any cleanup or additional actions before exiting the application
            Application.Exit();
        }

        private void AddExitButton()
        {
            int buttonSpacing = 10;
            int buttonHeight = 30;

            buttonExit = new Button
            {
                Name = "buttonExit",
                Text = "Exit",
                Width = 75,
                Height = buttonHeight,
                Location = new Point(10, 40 + (buttonHeight + buttonSpacing) * (int)Direction.South * 2),
            };

            buttonExit.Click += buttonExit_Click;
            this.Controls.Add(buttonExit);
        }

        private void textBoxCommand_TextChanged(object sender, EventArgs e)
        {
            // Handle text changed event if needed
        }

    }
}