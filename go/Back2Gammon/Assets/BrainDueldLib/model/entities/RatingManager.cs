
using System.Collections.Generic; using System;

using System.Text;

namespace BrainDuelsLib.model.entities
{
    public class RatingManager
    {
        public static double DEFAULT_BEGINNING_RATING = 1500.0;

        public enum RatingCategory
        {
            DEFAULT,
            PAWN,
            BISHOP,
            ROOK,
            QUEEN,
            KING
        }

        public static RatingCategory GetCategory(double rating)
        {
            if (rating < DEFAULT_BEGINNING_RATING - 300)
            {
                return RatingCategory.PAWN;
            }
            if (rating >= DEFAULT_BEGINNING_RATING - 300 && rating < DEFAULT_BEGINNING_RATING - 100)
            {
                return RatingCategory.BISHOP;
            }
            if (rating >= DEFAULT_BEGINNING_RATING - 100 && rating < DEFAULT_BEGINNING_RATING + 100)
            {
                return RatingCategory.ROOK;
            }
            if (rating >= DEFAULT_BEGINNING_RATING + 100 && rating < DEFAULT_BEGINNING_RATING + 300)
            {
                return RatingCategory.QUEEN;
            }
            if (rating >= DEFAULT_BEGINNING_RATING + 300)
            {
                return RatingCategory.KING;
            }
            throw new ArgumentException();
        }

        public static int GetColor(double rating)
        {
            RatingCategory category = GetCategory(rating);
            if (category == RatingCategory.PAWN)
            {
                return 0xAAAAAA;
            }

            if (category == RatingCategory.BISHOP)
            {
                return 0x55FF55;
            }

            if (category == RatingCategory.ROOK)
            {
                return 0x5555FF;
            }

            if (category == RatingCategory.QUEEN)
            {
                return 0xFFFF55;
            }

            if (category == RatingCategory.KING)
            {
                return 0xFF5555;
            }

            throw new ArgumentException();
        }

        public static double K = 40;
        public static double GetEloDifference(double me, double opponent, double actualPoints){
            double e = 1.0/(1+Math.Pow(10.0, (opponent - me)/400.0));
            double diff = K*(actualPoints - e);
            return diff;
        }
    }
}
