/* Name:     David A. Clark, Jr.
 * Student:  0004796375
 * Class:    Development Portfolio 3 (MDV239-O)
 * Term:     C201904-01
 * Exercise: Event Handlers
 * Date:     April 5, 2019 */

using System;

namespace ClarkDavid_Assignment1
{
    public class AddedEditedCourse : EventArgs
    {
        // Custom Event Property
        public string Name { get; private set; }

        // Event Constructor
        public AddedEditedCourse( string name ) => Name = name;
    }
}
