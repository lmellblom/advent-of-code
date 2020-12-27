using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December25 : AoCSolver
    {
        public December25() : base(25)
        {
        }

        public record Cryptographic(long PublicKey)
        {
            public int SecretLoopSize { get; set; }

            public void DetermineLoopSize(int initialSubject = 7)
            {
                SecretLoopSize = -1;

                // brut force!!
                var until = 1000000000;
                long number = 1;
                for (int loop = 1; loop <= until; loop++)
                {
                    number = Transform(number, initialSubject);
                    if (number == PublicKey)
                    {
                        SecretLoopSize = loop;
                        return;
                    }
                }
            }

            public long EncryptOtherPublicKey(long publicKey)
            {
                return TransformNumber(publicKey, SecretLoopSize);
            }

            private long TransformNumber(long subjectNr, int loopSize)
            {
                long number = 1;

                while (loopSize-- > 0)
                {
                    number = Transform(number, subjectNr);
                }

                return number;
            }

            private long Transform(long currNumber, long inputNr)
            {
                currNumber *= inputNr;
                currNumber %= 20201227;
                return currNumber;
            }
        }

        public class ComboBreaker
        {
            // The handshake used by the card and the door involves an operation 
            // that transforms a subject number. To transform a subject number, 
            // start with the value 1. Then, a number of times called the loop size,
            // perform the following steps:
            // Set the value to itself multiplied by the subject number.
            // Set the value to the remainder after dividing the value by 20201227.

            public Cryptographic Card { get; set; }
            public Cryptographic Door { get; set; }

            public long EncryptionKey { get; set; }

            public ComboBreaker(long key1, long key2)
            {
                Card = new Cryptographic(key1);
                Door = new Cryptographic(key2);

                EncryptionKey = -1;

                Card.DetermineLoopSize();
                Door.DetermineLoopSize();

                var encrypDoor = Card.EncryptOtherPublicKey(Door.PublicKey);
                var encrypCard = Door.EncryptOtherPublicKey(Card.PublicKey);

                if (encrypCard == encrypDoor)
                {
                    EncryptionKey = encrypCard;
                }
            }
        }

        public override bool Test()
        {
            var breaker = new ComboBreaker(5764801, 17807724);
            bool testSucceeded = breaker.EncryptionKey == 14897079;
            return testSucceeded;
        }

        public override string First()
        {
            var breaker = new ComboBreaker(8252394, 6269621);
            return breaker.EncryptionKey.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            bool testSucceeded = false;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            return "not implemented";
        }
    }
}
