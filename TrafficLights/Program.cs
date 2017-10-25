using System;
using System.Text.RegularExpressions;

namespace TrafficLights {
    class Program {
        static void Main(string[] args) {
            int greenTime, yellowTime, redTime;
            int action;
            string str;
            Console.WriteLine("============交通信号灯============");
            Input:
            while (true) {
                Console.Write("请输入绿灯持续时间：");
                str = Console.ReadLine();
                if (InputVerify(str)) {
                    greenTime = Convert.ToUInt16(str);
                    break;
                }
            }
            while (true) {
                Console.Write("请输入黄灯持续时间：");
                str = Console.ReadLine();
                if (InputVerify(str)) {
                    yellowTime = Convert.ToUInt16(str);
                    break;
                }
            }
            while (true) {
                Console.Write("请输入红灯持续时间：");
                str = Console.ReadLine();
                if (InputVerify(str)) {
                    redTime = Convert.ToUInt16(str);
                    break;
                }
            }
            while (true) {
                for (int i = greenTime; i > 0; i--) {
                    Draw(i, ConsoleColor.Green);
                    action = TrafficLight.Status();
                    switch (action) {
                        case 2:
                            Console.Clear();
                            goto Input;
                        case 3:
                            goto End;
                        default:
                            continue;
                    }
                }
                for (int i = yellowTime; i > 0; i--) {
                    Draw(i, ConsoleColor.Yellow);
                    action = TrafficLight.Status();
                    switch (action) {
                        case 2:
                            Console.Clear();
                            goto Input;
                        case 3:
                            goto End;
                        default:
                            continue;
                    }
                }
                for (int i = redTime; i > 0; i--) {
                    Draw(i, ConsoleColor.Red);
                    action = TrafficLight.Status();
                    switch (action) {
                        case 2:
                            Console.Clear();
                            goto Input;
                        case 3:
                            goto End;
                        default:
                            continue;
                    }
                }
            }
            End:
            Console.WriteLine("\n程序结束，按任意键退出。");
            Console.ReadKey();
        }

        static void Draw(int i, ConsoleColor color) {
            Console.Clear();
            TrafficLight traffic = new TrafficLight(i);
            int rowLen = traffic.leftLight.GetLength(0);
            int colLen = traffic.rightLight.GetLength(1);
            for (int row = 0; row < rowLen; row++) {
                Console.Write("\t");
                for (int col = 0; col < colLen; col++) {
                    if (traffic.leftLight[row, col]) {
                        Console.ForegroundColor = color;
                    } else {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write('●');
                }
                Console.Write("\t");
                for (int col = 0; col < colLen; col++) {
                    if (traffic.rightLight[row, col]) {
                        Console.ForegroundColor = color;
                    } else {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write('●');
                }
                Console.Write('\n');
            }
            Console.WriteLine("\n\n");
            Console.WriteLine("【暂停/启动：Space；重新设定时间：Enter；结束：Esc】");
        }

        static bool InputVerify(string txt) {
            Regex regex = new Regex(@"^[0-9]*$");
            if (string.IsNullOrEmpty(txt)) {
                Console.WriteLine("没有任何输入，请输入0~99的整数！");
                return false;
            }
            if (!regex.IsMatch(txt) || Convert.ToInt16(txt) > 100 || Convert.ToInt16(txt) < 0) {
                Console.WriteLine("请输入0~99的整数！");
                return false;
            }
            return true;
        }
    }

    class TrafficLight {
        public bool[,] light = new bool[20, 10];
        public bool[,] leftLight = new bool[20, 10];
        public bool[,] rightLight = new bool[20, 10];

        public TrafficLight(int number) {
            int n = number;
            for (int i = 0; i < 2; i++) {
                switch (i) {
                    case 0:
                        this.light = this.leftLight;
                        n = number < 10 ? 0 : number / 10;
                        break;
                    case 1:
                        this.light = this.rightLight;
                        n = number < 10 ? number : number % 10;
                        break;
                }
                switch (n) {
                    case 0:
                        this.LeftTop();
                        this.Top();
                        this.RightTop();
                        this.LeftBottom();
                        this.Bottom();
                        this.RightBottom();
                        break;
                    case 1:
                        this.RightTop();
                        this.RightBottom();
                        break;
                    case 2:
                        this.Top();
                        this.RightTop();
                        this.Middle();
                        this.LeftBottom();
                        this.Bottom();
                        break;
                    case 3:
                        this.Top();
                        this.RightTop();
                        this.Middle();
                        this.RightBottom();
                        this.Bottom();
                        break;
                    case 4:
                        this.LeftTop();
                        this.Middle();
                        this.RightTop();
                        this.RightBottom();
                        break;
                    case 5:
                        this.Top();
                        this.LeftTop();
                        this.Middle();
                        this.RightBottom();
                        this.Bottom();
                        break;
                    case 6:
                        this.Top();
                        this.LeftTop();
                        this.Middle();
                        this.LeftBottom();
                        this.RightBottom();
                        this.Bottom();
                        break;
                    case 7:
                        this.Top();
                        this.RightTop();
                        this.RightBottom();
                        break;
                    case 8:
                        this.Top();
                        this.RightTop();
                        this.LeftTop();
                        this.Middle();
                        this.LeftBottom();
                        this.RightBottom();
                        this.Bottom();
                        break;
                    case 9:
                        this.Top();
                        this.LeftTop();
                        this.RightTop();
                        this.Middle();
                        this.RightBottom();
                        this.Bottom();
                        break;
                }
            }
        }

        public static int Status() {
            int flag = 1;
            while (true) {
                System.Threading.Thread.Sleep(1000);

                // 检测键盘响应
                while (Console.KeyAvailable) {
                    ConsoleKeyInfo info = Console.ReadKey();
                    switch (info.Key) {
                        case ConsoleKey.Spacebar:
                            flag = flag == 1 ? 2 : 1;
                            break;
                        case ConsoleKey.Enter:
                            flag = 3;
                            break;
                        case ConsoleKey.Escape:
                            flag = 4;
                            break;
                    }
                }
                switch (flag) {
                    case 1: // 继续
                        return 1;
                    case 2: // 暂停
                        break;
                    case 3: // 重新设置时间
                        return 2;
                    case 4: // 结束退出
                        return 3;
                    default:
                        break;
                }
            }
        }

        // 将数字显示分为 左上 上 右上 中 左下 下 右下
        // true 显示颜色，false 颜色不变
        // 左上
        private void LeftTop() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (!light[row, col] == true && row < 10 && col < 2) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 上
        private void Top() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (!light[row, col] == true && row < 2) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 右上
        private void RightTop() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (light[row, col] == false && row < light.GetLength(0) / 2 && col > light.GetLength(1) - 3) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 中
        private void Middle() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (light[row, col] == false && row == light.GetLength(0) / 2 - 1 || row == light.GetLength(0) / 2) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 左下
        private void LeftBottom() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (light[row, col] == false && row >= 10 && col < 2) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 下
        private void Bottom() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (light[row, col] == false && row == light.GetLength(0) - 2 || row == light.GetLength(0) - 1) {
                        light[row, col] = true;
                    }
                }
            }
        }

        // 右下
        private void RightBottom() {
            for (int row = 0; row < light.GetLength(0); row++) {
                for (int col = 0; col < light.GetLength(1); col++) {
                    if (light[row, col] == false && row >= light.GetLength(0) / 2 && col > light.GetLength(1) - 3) {
                        light[row, col] = true;
                    }
                }
            }
        }
    }
}
