using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Storage.Images
{
    public interface ImageFileRepository
    {
        // Methods
        Uri Add(ImageFile imageFile, string directoryPath = null);

        void Remove(Uri imageUri);
    }
}
