namespace Komp_lab1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.butt_about_program = new System.Windows.Forms.Button();
            this.butt_help = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.butt_paste = new System.Windows.Forms.Button();
            this.butt_cut = new System.Windows.Forms.Button();
            this.butt_copy = new System.Windows.Forms.Button();
            this.butt_repeat = new System.Windows.Forms.Button();
            this.butt_cancel = new System.Windows.Forms.Button();
            this.butt_save_file = new System.Windows.Forms.Button();
            this.butt_open_file = new System.Windows.Forms.Button();
            this.butt_new_file = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.открытьФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.openTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.Output = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.repeatTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.cutTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.selectallTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.запуститьСкриптToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.постановкаЗадачиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.грамматикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.классификацияГрамматикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.методАнализаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.тестовыйПримерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокЛитературыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.исходныйКодПрограммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.готовыеЗапросыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CallingHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.butt_about_program);
            this.panel1.Controls.Add(this.butt_help);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.butt_paste);
            this.panel1.Controls.Add(this.butt_cut);
            this.panel1.Controls.Add(this.butt_copy);
            this.panel1.Controls.Add(this.butt_repeat);
            this.panel1.Controls.Add(this.butt_cancel);
            this.panel1.Controls.Add(this.butt_save_file);
            this.panel1.Controls.Add(this.butt_open_file);
            this.panel1.Controls.Add(this.butt_new_file);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 96);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "label1";
            // 
            // butt_about_program
            // 
            this.butt_about_program.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_about_program.BackgroundImage")));
            this.butt_about_program.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_about_program.Location = new System.Drawing.Point(648, 32);
            this.butt_about_program.Name = "butt_about_program";
            this.butt_about_program.Size = new System.Drawing.Size(58, 33);
            this.butt_about_program.TabIndex = 12;
            this.butt_about_program.UseVisualStyleBackColor = true;
            this.butt_about_program.Click += new System.EventHandler(this.butt_about_program_Click);
            // 
            // butt_help
            // 
            this.butt_help.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_help.BackgroundImage")));
            this.butt_help.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_help.Location = new System.Drawing.Point(585, 32);
            this.butt_help.Name = "butt_help";
            this.butt_help.Size = new System.Drawing.Size(57, 33);
            this.butt_help.TabIndex = 11;
            this.butt_help.UseVisualStyleBackColor = true;
            this.butt_help.Click += new System.EventHandler(this.butt_help_Click);
            // 
            // button9
            // 
            this.button9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button9.BackgroundImage")));
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button9.Location = new System.Drawing.Point(537, 32);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(42, 33);
            this.button9.TabIndex = 10;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // butt_paste
            // 
            this.butt_paste.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_paste.BackgroundImage")));
            this.butt_paste.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_paste.Location = new System.Drawing.Point(489, 32);
            this.butt_paste.Name = "butt_paste";
            this.butt_paste.Size = new System.Drawing.Size(42, 33);
            this.butt_paste.TabIndex = 9;
            this.butt_paste.UseVisualStyleBackColor = true;
            this.butt_paste.Click += new System.EventHandler(this.butt_paste_Click);
            // 
            // butt_cut
            // 
            this.butt_cut.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_cut.BackgroundImage")));
            this.butt_cut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_cut.Location = new System.Drawing.Point(434, 32);
            this.butt_cut.Name = "butt_cut";
            this.butt_cut.Size = new System.Drawing.Size(49, 33);
            this.butt_cut.TabIndex = 8;
            this.butt_cut.UseVisualStyleBackColor = true;
            this.butt_cut.Click += new System.EventHandler(this.butt_cut_Click);
            // 
            // butt_copy
            // 
            this.butt_copy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_copy.BackgroundImage")));
            this.butt_copy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_copy.Location = new System.Drawing.Point(384, 32);
            this.butt_copy.Name = "butt_copy";
            this.butt_copy.Size = new System.Drawing.Size(44, 33);
            this.butt_copy.TabIndex = 7;
            this.butt_copy.UseVisualStyleBackColor = true;
            this.butt_copy.Click += new System.EventHandler(this.butt_copy_Click);
            // 
            // butt_repeat
            // 
            this.butt_repeat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_repeat.BackgroundImage")));
            this.butt_repeat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_repeat.Location = new System.Drawing.Point(295, 31);
            this.butt_repeat.Name = "butt_repeat";
            this.butt_repeat.Size = new System.Drawing.Size(67, 33);
            this.butt_repeat.TabIndex = 6;
            this.butt_repeat.UseVisualStyleBackColor = true;
            this.butt_repeat.Click += new System.EventHandler(this.butt_repeat_Click);
            // 
            // butt_cancel
            // 
            this.butt_cancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_cancel.BackgroundImage")));
            this.butt_cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_cancel.Location = new System.Drawing.Point(222, 31);
            this.butt_cancel.Name = "butt_cancel";
            this.butt_cancel.Size = new System.Drawing.Size(67, 33);
            this.butt_cancel.TabIndex = 5;
            this.butt_cancel.UseVisualStyleBackColor = true;
            this.butt_cancel.Click += new System.EventHandler(this.butt_cancel_Click);
            // 
            // butt_save_file
            // 
            this.butt_save_file.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_save_file.BackgroundImage")));
            this.butt_save_file.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butt_save_file.Location = new System.Drawing.Point(104, 31);
            this.butt_save_file.Name = "butt_save_file";
            this.butt_save_file.Size = new System.Drawing.Size(48, 33);
            this.butt_save_file.TabIndex = 4;
            this.butt_save_file.UseVisualStyleBackColor = true;
            this.butt_save_file.Click += new System.EventHandler(this.butt_save_file_Click);
            // 
            // butt_open_file
            // 
            this.butt_open_file.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_open_file.BackgroundImage")));
            this.butt_open_file.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butt_open_file.Location = new System.Drawing.Point(56, 31);
            this.butt_open_file.Name = "butt_open_file";
            this.butt_open_file.Size = new System.Drawing.Size(42, 33);
            this.butt_open_file.TabIndex = 3;
            this.butt_open_file.UseVisualStyleBackColor = true;
            this.butt_open_file.Click += new System.EventHandler(this.butt_open_file_Click);
            // 
            // butt_new_file
            // 
            this.butt_new_file.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("butt_new_file.BackgroundImage")));
            this.butt_new_file.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butt_new_file.Location = new System.Drawing.Point(14, 32);
            this.butt_new_file.Name = "butt_new_file";
            this.butt_new_file.Size = new System.Drawing.Size(36, 33);
            this.butt_new_file.TabIndex = 2;
            this.butt_new_file.UseVisualStyleBackColor = true;
            this.butt_new_file.Click += new System.EventHandler(this.butt_new_file_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьФайлToolStripMenuItem,
            this.сохранитьФайлToolStripMenuItem,
            this.запуститьСкриптToolStripMenuItem,
            this.готовыеЗапросыToolStripMenuItem,
            this.OutputTSM,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(882, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // открытьФайлToolStripMenuItem
            // 
            this.открытьФайлToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.открытьФайлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTSM,
            this.openTSM,
            this.saveTSM,
            this.SaveAsTSM,
            this.Output});
            this.открытьФайлToolStripMenuItem.Name = "открытьФайлToolStripMenuItem";
            this.открытьФайлToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.открытьФайлToolStripMenuItem.Text = "Файл";
            // 
            // createTSM
            // 
            this.createTSM.Name = "createTSM";
            this.createTSM.Size = new System.Drawing.Size(201, 26);
            this.createTSM.Text = "Создать ";
            this.createTSM.Click += new System.EventHandler(this.createTSM_Click);
            // 
            // openTSM
            // 
            this.openTSM.Name = "openTSM";
            this.openTSM.Size = new System.Drawing.Size(201, 26);
            this.openTSM.Text = "Открыть";
            this.openTSM.Click += new System.EventHandler(this.openTSM_Click);
            // 
            // saveTSM
            // 
            this.saveTSM.Name = "saveTSM";
            this.saveTSM.Size = new System.Drawing.Size(201, 26);
            this.saveTSM.Text = "Сохранить";
            this.saveTSM.Click += new System.EventHandler(this.saveTSM_Click);
            // 
            // SaveAsTSM
            // 
            this.SaveAsTSM.Name = "SaveAsTSM";
            this.SaveAsTSM.Size = new System.Drawing.Size(201, 26);
            this.SaveAsTSM.Text = "Сохранить как...";
            this.SaveAsTSM.Click += new System.EventHandler(this.SaveAsTSM_Click);
            // 
            // Output
            // 
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(201, 26);
            this.Output.Text = "Выход";
            this.Output.Click += new System.EventHandler(this.Output_Click);
            // 
            // сохранитьФайлToolStripMenuItem
            // 
            this.сохранитьФайлToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.сохранитьФайлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelTSM,
            this.repeatTSM,
            this.cutTSM,
            this.copyTSM,
            this.pasteTSM,
            this.removeTSM,
            this.selectallTSM});
            this.сохранитьФайлToolStripMenuItem.Name = "сохранитьФайлToolStripMenuItem";
            this.сохранитьФайлToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.сохранитьФайлToolStripMenuItem.Text = "Правка";
            // 
            // cancelTSM
            // 
            this.cancelTSM.Name = "cancelTSM";
            this.cancelTSM.Size = new System.Drawing.Size(190, 26);
            this.cancelTSM.Text = "Отмена";
            this.cancelTSM.Click += new System.EventHandler(this.cancelTSM_Click);
            // 
            // repeatTSM
            // 
            this.repeatTSM.Name = "repeatTSM";
            this.repeatTSM.Size = new System.Drawing.Size(190, 26);
            this.repeatTSM.Text = "Вернуть";
            this.repeatTSM.Click += new System.EventHandler(this.repeatTSM_Click);
            // 
            // cutTSM
            // 
            this.cutTSM.Name = "cutTSM";
            this.cutTSM.Size = new System.Drawing.Size(190, 26);
            this.cutTSM.Text = "Вырезать";
            this.cutTSM.Click += new System.EventHandler(this.cutTSM_Click);
            // 
            // copyTSM
            // 
            this.copyTSM.Name = "copyTSM";
            this.copyTSM.Size = new System.Drawing.Size(190, 26);
            this.copyTSM.Text = "Копировать";
            this.copyTSM.Click += new System.EventHandler(this.copyTSM_Click);
            // 
            // pasteTSM
            // 
            this.pasteTSM.Name = "pasteTSM";
            this.pasteTSM.Size = new System.Drawing.Size(190, 26);
            this.pasteTSM.Text = "Вставить";
            this.pasteTSM.Click += new System.EventHandler(this.pasteTSM_Click);
            // 
            // removeTSM
            // 
            this.removeTSM.Name = "removeTSM";
            this.removeTSM.Size = new System.Drawing.Size(190, 26);
            this.removeTSM.Text = "Удалить";
            this.removeTSM.Click += new System.EventHandler(this.removeTSM_Click);
            // 
            // selectallTSM
            // 
            this.selectallTSM.Name = "selectallTSM";
            this.selectallTSM.Size = new System.Drawing.Size(190, 26);
            this.selectallTSM.Text = "Выделить все ";
            this.selectallTSM.Click += new System.EventHandler(this.selectallTSM_Click);
            // 
            // запуститьСкриптToolStripMenuItem
            // 
            this.запуститьСкриптToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.запуститьСкриптToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.постановкаЗадачиToolStripMenuItem,
            this.грамматикаToolStripMenuItem,
            this.классификацияГрамматикиToolStripMenuItem,
            this.методАнализаToolStripMenuItem,
            this.тестовыйПримерToolStripMenuItem,
            this.списокЛитературыToolStripMenuItem,
            this.исходныйКодПрограммыToolStripMenuItem});
            this.запуститьСкриптToolStripMenuItem.Name = "запуститьСкриптToolStripMenuItem";
            this.запуститьСкриптToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.запуститьСкриптToolStripMenuItem.Text = "Текст";
            // 
            // постановкаЗадачиToolStripMenuItem
            // 
            this.постановкаЗадачиToolStripMenuItem.Name = "постановкаЗадачиToolStripMenuItem";
            this.постановкаЗадачиToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.постановкаЗадачиToolStripMenuItem.Text = "Постановка задачи";
            // 
            // грамматикаToolStripMenuItem
            // 
            this.грамматикаToolStripMenuItem.Name = "грамматикаToolStripMenuItem";
            this.грамматикаToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.грамматикаToolStripMenuItem.Text = "Грамматика";
            // 
            // классификацияГрамматикиToolStripMenuItem
            // 
            this.классификацияГрамматикиToolStripMenuItem.Name = "классификацияГрамматикиToolStripMenuItem";
            this.классификацияГрамматикиToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.классификацияГрамматикиToolStripMenuItem.Text = "Классификация грамматики";
            // 
            // методАнализаToolStripMenuItem
            // 
            this.методАнализаToolStripMenuItem.Name = "методАнализаToolStripMenuItem";
            this.методАнализаToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.методАнализаToolStripMenuItem.Text = "Метод анализа";
            // 
            // тестовыйПримерToolStripMenuItem
            // 
            this.тестовыйПримерToolStripMenuItem.Name = "тестовыйПримерToolStripMenuItem";
            this.тестовыйПримерToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.тестовыйПримерToolStripMenuItem.Text = "Тестовый пример";
            // 
            // списокЛитературыToolStripMenuItem
            // 
            this.списокЛитературыToolStripMenuItem.Name = "списокЛитературыToolStripMenuItem";
            this.списокЛитературыToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.списокЛитературыToolStripMenuItem.Text = "Список литературы";
            // 
            // исходныйКодПрограммыToolStripMenuItem
            // 
            this.исходныйКодПрограммыToolStripMenuItem.Name = "исходныйКодПрограммыToolStripMenuItem";
            this.исходныйКодПрограммыToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.исходныйКодПрограммыToolStripMenuItem.Text = "Исходный код программы";
            // 
            // готовыеЗапросыToolStripMenuItem
            // 
            this.готовыеЗапросыToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.готовыеЗапросыToolStripMenuItem.Name = "готовыеЗапросыToolStripMenuItem";
            this.готовыеЗапросыToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.готовыеЗапросыToolStripMenuItem.Text = "Пуск";
            // 
            // OutputTSM
            // 
            this.OutputTSM.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.OutputTSM.BackColor = System.Drawing.Color.Transparent;
            this.OutputTSM.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OutputTSM.BackgroundImage")));
            this.OutputTSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OutputTSM.Name = "OutputTSM";
            this.OutputTSM.Size = new System.Drawing.Size(39, 24);
            this.OutputTSM.Text = "    ";
            this.OutputTSM.Click += new System.EventHandler(this.OutputTSM_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CallingHelp,
            this.AboutProgram});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // CallingHelp
            // 
            this.CallingHelp.Name = "CallingHelp";
            this.CallingHelp.Size = new System.Drawing.Size(224, 26);
            this.CallingHelp.Text = "Вызов справки";
            this.CallingHelp.Click += new System.EventHandler(this.CallingHelp_Click);
            // 
            // AboutProgram
            // 
            this.AboutProgram.Name = "AboutProgram";
            this.AboutProgram.Size = new System.Drawing.Size(224, 26);
            this.AboutProgram.Text = "О программе";
            this.AboutProgram.Click += new System.EventHandler(this.AboutProgram_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(98, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(784, 269);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(238)))), ((int)(((byte)(218)))));
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox2.Location = new System.Drawing.Point(0, 0);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox2.Size = new System.Drawing.Size(98, 269);
            this.richTextBox2.TabIndex = 13;
            this.richTextBox2.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(882, 184);
            this.dataGridView1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 96);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(882, 457);
            this.splitContainer1.SplitterDistance = 269;
            this.splitContainer1.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Form1";
            this.Text = "Text Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem открытьФайлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьФайлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запуститьСкриптToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem готовыеЗапросыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OutputTSM;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.Button butt_paste;
        private System.Windows.Forms.Button butt_cut;
        private System.Windows.Forms.Button butt_copy;
        private System.Windows.Forms.Button butt_repeat;
        private System.Windows.Forms.Button butt_cancel;
        private System.Windows.Forms.Button butt_save_file;
        private System.Windows.Forms.Button butt_open_file;
        private System.Windows.Forms.Button butt_new_file;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button butt_help;
        private System.Windows.Forms.Button butt_about_program;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ToolStripMenuItem createTSM;
        private System.Windows.Forms.ToolStripMenuItem openTSM;
        private System.Windows.Forms.ToolStripMenuItem saveTSM;
        private System.Windows.Forms.ToolStripMenuItem SaveAsTSM;
        private System.Windows.Forms.ToolStripMenuItem Output;
        private System.Windows.Forms.ToolStripMenuItem cancelTSM;
        private System.Windows.Forms.ToolStripMenuItem repeatTSM;
        private System.Windows.Forms.ToolStripMenuItem cutTSM;
        private System.Windows.Forms.ToolStripMenuItem copyTSM;
        private System.Windows.Forms.ToolStripMenuItem pasteTSM;
        private System.Windows.Forms.ToolStripMenuItem removeTSM;
        private System.Windows.Forms.ToolStripMenuItem selectallTSM;
        private System.Windows.Forms.ToolStripMenuItem постановкаЗадачиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem грамматикаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem классификацияГрамматикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem методАнализаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тестовыйПримерToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокЛитературыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem исходныйКодПрограммыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CallingHelp;
        private System.Windows.Forms.ToolStripMenuItem AboutProgram;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

