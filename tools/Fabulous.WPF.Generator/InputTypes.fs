namespace Fabulous.WPF

[<AutoOpen>]
module InputTypes =
    /// Represents an image source
    type Image =
        /// A path to the image
        | Path of string
        /// A byte array representing the image
        | Bytes of byte[]
        /// An already defined ImageSource
        | Source of System.Windows.Media.ImageSource