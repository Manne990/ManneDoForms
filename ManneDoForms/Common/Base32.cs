using System.Text;

namespace ManneDoForms.Common
{
    public static class Base32
    {
    	// the valid chars for the encoding
    	private const string ValidChars = "QAZ2WSX3" + "EDC4RFV5" + "TGB6YHN7" + "UJM8K9LP";

    	/// <summary>
    	/// Converts an array of bytes to a Base32-k string.
    	/// </summary>
    	public static string ToBase32String(byte[] bytes)
    	{
    		var sb = new StringBuilder();         // holds the base32 chars
    		var hi = 5;
    		var currentByte = 0;

    		while (currentByte < bytes.Length)
    		{
    			// do we need to use the next byte?
    			byte index;
    			if (hi > 8)
    			{
    				// get the last piece from the current byte, shift it to the right
    				// and increment the byte counter
    				index = (byte)(bytes[currentByte++] >> (hi - 5));
    				if (currentByte != bytes.Length)
    				{
    					// if we are not at the end, get the first piece from
    					// the next byte, clear it and shift it to the left
    					index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
    				}

    				hi -= 3;
    			}
    			else if (hi == 8)
    			{
    				index = (byte)(bytes[currentByte++] >> 3);
    				hi -= 3;
    			}
    			else
    			{

    				// simply get the stuff from the current byte
    				index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
    				hi += 5;
    			}

    			sb.Append(ValidChars[index]);
    		}

    		return sb.ToString();
    	}
    }
}