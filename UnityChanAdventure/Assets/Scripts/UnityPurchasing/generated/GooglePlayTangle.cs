// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("372Zfk9wPU3nzLIpXRX6xjJA6W92lr2DKsd0/bZILr8COsrEaDScwpufE3AOvQjEae4G19ytAyWWHoDzHyxHXP0aMleUsmcOHpgcGdsyhe0tnxw/LRAbFDebVZvqEBwcHBgdHvIoz8VbC9tqF1zInpN1NX82M+xJ/lwCXRxaMlRoFuXT8yG3dqXNgimfHBIdLZ8cFx+fHBwdnc2SdMfmGAkmYO12kQsPlJlrlRjTatkWghQDbYhuKI1MlhHpD4p1yBmeTlS09/mcRSS/FEj9WmZ6iGGMYLgXVuCilsOt+T6QG9bF9KkR25CcM1bzKkpXPPvj8QV0Gp1Ujv5cOWZbet3Lz8Xp5Re7hoLFEzMhKcRuZRqD5vVJ7mdfiSFtMf54FB8eHB0c");
        private static int[] order = new int[] { 11,3,10,4,11,13,10,11,10,13,11,11,13,13,14 };
        private static int key = 29;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
