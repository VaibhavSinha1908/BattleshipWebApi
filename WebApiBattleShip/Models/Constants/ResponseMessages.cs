namespace WebApiBattleShip.Models
{
    public static class ResponseMessages
    {
        public const string INVALID_REQUEST = "The request format was invalid.";

        public const string BOARD_CREATED = "The 10x10 board has been created.";

        public const string BOARD_NOT_CREATED = "The 10x10 board was not created.";

        public const string BOARD_CREATE_FAILED = "The board creation failed.";

        public const string SHIP_ADDED = "The ship has been added.";

        public const string SHIP_OVERLAPPING = "The ship cannot be added as it coincides with another ship.";

        public const string SHIP_OUTOF_BOUNDS = "The ship cannot be added as coordinates are out of bounds.";

        public const string SHIP_NOT_ADDED = "The ship cannot be added.";

        public const string ATTACK_SUCCESSFUL = "The attack hit a ship.";

        public const string ATTACK_MISS = "The attack missed a ship.";

        public const string ATTACK_SUNK_SHIP = "The attack sunk a ship.";







    }
}
