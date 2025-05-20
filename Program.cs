using System;
using System.Collections.Generic;
using System.Threading;

namespace CarRacingGame
{
    public abstract class Car
    {
        public string Name { get; protected set; }
        public int Speed { get; protected set; }
        public int Position { get; protected set; } = 0;

        public delegate void FinishEventHandler(Car car);
        public event FinishEventHandler OnFinish;

        protected Random random = new Random();

        public Car(string name)
        {
            Name = name;
        }
        protected void RaiseFinishEvent()
        {
            OnFinish?.Invoke(this);
        }
        public virtual void Drive()
        {
            int deltaSpeed = random.Next(5, 16);
            Speed = deltaSpeed;
            Position += Speed;
            if (Position >= 100)
            {
                Position = 100;
                RaiseFinishEvent();
            }
        }

        public override string ToString()
        {
            return $"{Name} (Положение: {Position})";
        }
    } 
    public class SportCar : Car
    {
        public SportCar(string name) : base(name) { }

        public override void Drive()
        {
            int deltaSpeed = random.Next(10, 20);
            Speed = deltaSpeed;
            Position += Speed;
            if (Position >= 100)
            {
                Position = 100;
                RaiseFinishEvent();
            }
        }
    }

    public class Sedan : Car
    {
        public Sedan(string name) : base(name) { }

        public override void Drive()
        {
            int deltaSpeed = random.Next(5, 15);
            Speed = deltaSpeed;
            Position += Speed;
            if (Position >= 100)
            {
                Position = 100;
                RaiseFinishEvent();
            }
        }
    }

    public class Truck : Car
    {
        public Truck(string name) : base(name) { }

        public override void Drive()
        {
            int deltaSpeed = random.Next(3, 10);
            Speed = deltaSpeed;
            Position += Speed;
            if (Position >= 100)
            {
                Position = 100;
                RaiseFinishEvent();
            }
        }
    }

    public class Bus : Car
    {
        public Bus(string name) : base(name) { }

        public override void Drive()
        {
            int deltaSpeed = random.Next(4, 12);
            Speed = deltaSpeed;
            Position += Speed;
            if (Position >= 100)
            {
                Position = 100;
                RaiseFinishEvent();
            }
        }
    } 
    public class Race
    {
        private List<Car> cars = new List<Car>();
        private bool raceFinished = false;
        private Random rand = new Random();
        public delegate void UpdatePositionHandler(Car car);
        public event UpdatePositionHandler OnUpdate;
        public Race()
        {
            cars.Add(new SportCar("Спорткар 1"));
            cars.Add(new Sedan("Легковой 1"));
            cars.Add(new Truck("Грузовик 1"));
            cars.Add(new Bus("Автобус 1"));
            foreach (var car in cars)
            {
                car.OnFinish += Car_Finished;
            }
        }

        private void Car_Finished(Car car)
        {
            raceFinished = true;
            Console.WriteLine($"\n{car.Name} достиг финиша и победил!");
        }

        public void StartRace()
        {
            Console.WriteLine("Гонка началась!\n");
            while (!raceFinished)
            {
                ShuffleCars();
                foreach (var car in cars)
                {
                    car.Drive();
                    Console.WriteLine(car);
                    OnUpdate?.Invoke(car);
                    if (car.Position >= 100)
                    {
                        raceFinished = true;
                        break;
                    }
                }
                Console.WriteLine("------------------------------");
                Thread.Sleep(500);
            }
            Console.WriteLine("\nГонка завершена!");
        }

        private void ShuffleCars()
        {
            int n = cars.Count;
            for (int i = 0; i < n; i++)
            {
                int j = rand.Next(i, n);
                (cars[i], cars[j]) = (cars[j], cars[i]);
            }
        }
    }

    // Основная программа
    class Program
    {
        static void Main()
        {
            Race race = new Race();
            race.StartRace();
        }
    }
}
