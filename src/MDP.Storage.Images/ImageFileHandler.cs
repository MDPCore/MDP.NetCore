using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Storage.Images
{
    public interface ImageFileHandler
    {
        // Methods
        ImageFile Process(ImageFile imageFile);
    }
}
