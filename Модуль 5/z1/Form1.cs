using System;                       
using System.Drawing;              
using System.Windows.Forms;         

namespace z1
{
    public partial class Form1 : Form
    {
        // Конструктор формы
        public Form1()
        {
            InitializeComponent();

            // Подписка на события мыши
            panelCanvas.MouseDown += PanelCanvas_MouseDown;
            panelCanvas.MouseMove += PanelCanvas_MouseMove;
            panelCanvas.MouseUp += PanelCanvas_MouseUp;
            panelCanvas.Paint += PanelCanvas_Paint;
        }

        // Кнопки выбора фигур
        // Смена типа фигуры в DrawingLogic
        private void btnLine_Click(object sender, EventArgs e)
            => DrawingLogic.CurrentShape = ShapeType.Line;

        private void btnCircle_Click(object sender, EventArgs e)
            => DrawingLogic.CurrentShape = ShapeType.Circle;

        private void btnRectangle_Click(object sender, EventArgs e)
            => DrawingLogic.CurrentShape = ShapeType.Rectangle;

        // Кнопка "Очистить"
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            DrawingLogic.ClearAll();

            panelCanvas.Invalidate();
        }

        // События мыши

        // Нажатие — начало рисования
        private void PanelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // ВЫзов метода из другого файла
                DrawingLogic.StartDrawing(e.Location);
            }
        }

        // Движение — обновление конечной точки
        private void PanelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawingLogic.IsDrawing)
            {
                // Вызов метода из другого файла
                DrawingLogic.UpdateDrawing(e.Location);

                panelCanvas.Invalidate();
            }
        }

        // Отпускание — финальная точка, сохранение фигуры
        private void PanelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (DrawingLogic.IsDrawing && e.Button == MouseButtons.Left)
            {
                // Вызов метода из другого файла
                DrawingLogic.FinishDrawing(e.Location);

                panelCanvas.Invalidate();
            }
        }

        // Перерисовка холста
        private void PanelCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Вызов метода из другого файла
            DrawingLogic.DrawAll(e.Graphics);
        }
    }
}