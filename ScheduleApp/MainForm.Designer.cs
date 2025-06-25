namespace ScheduleApp;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Text = "Schedule";
        this.Size = new Size(1200, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(800, 500);
        this.Icon = SystemIcons.Application;
        
        SetupToolStrip();
        SetupMainPanel();
        SetupStatusBar();
    }

    private void SetupToolStrip()
    {
        toolStrip = new ToolStrip
        {
            Height = 80,
            BackColor = Color.FromArgb(64, 128, 255),
            ForeColor = Color.White,
        };

        lblTitle = new Label
        {
            Text = "Schedule App",
            Font = new Font("Arial", 16, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Location = new Point(10,45),
            Size = new Size(300,30)
        };

        var filterPanel = new Label
        {
            Location = new Point(10, 45),
            Size = new Size(1150, 700),
            BackColor = Color.Transparent
        };

        var lblSport = new Label
        {
            Text = "Sport",
            Location = new Point(0, 5),
            Size = new Size(80, 90),
            ForeColor = Color.White,
        };

        cmbSports = new ComboBox
        {
            Location = new Point(85, 3),
            Size = new Size(120, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbSports.Items.AddRange(new[] {"All sports","Football","CS2","Valorant","Dota 2","Lol"});
        cmbSports.SelectedIndex = 0;
        cmbSports.SelectedIndexChanged += (s, e) => LoadMatches();
        
        var lblDate = new Label
        {
            Text = "Date",
            Location = new Point(220,5),
            Size = new Size(40, 20),
            ForeColor = Color.White
        };
        
        dtpDate = new DateTimePicker
        {
            Location = new Point(270, 3),
            Size = new Size(120, 25),
            Value = DateTime.Today
        };
        
        dtpDate.ValueChanged += (s, e) => LoadMatches();

        chkFavoriteOnly = new CheckBox
        {
            Text = "Favorite Only",
            Location = new Point(400, 5),
            Size = new Size(180, 20),
            ForeColor = Color.Yellow,
            Font = new Font("Arial", 9, FontStyle.Bold),
        };
        chkFavoriteOnly.CheckedChanged += (s, e) => LoadMatches();

        btnManageFavorites = new Button
        {
            Text = "Manage Favorites",
            Location = new Point(590, 1),
            Size = new Size(150, 28),
            BackColor = Color.Orange,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
        };
        btnManageFavorites.Click += BtnManageFavorites_Click;

        btnRefresh = new Button
        {
            Text = "Refresh",
            Location = new Point(750, 1),
            Size = new Size(100, 28),
            BackColor = Color.Green,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnRefresh.Click += (s, e) =>
        {
            InitializeData();
            LoadMatches();
            MessageBox.Show("Schedule App Refresh", "Schedule App Refresh", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };
        
        filterPanel.Controls.AddRange(new Control[]
        {
            lblSport, cmbSports, lblDate, dtpDate, chkFavoriteOnly, btnManageFavorites, btnRefresh
        });

        var toolPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.FromArgb(64, 128, 255),
        };
        toolPanel.Controls.Add(lblTitle);
        toolPanel.Controls.Add(filterPanel);
        this.Controls.Add(toolPanel);
    }

    private void SetupMainPanel()
    {
        dgvMatches = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 10),
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.LightBlue,
                SelectionForeColor = Color.Black
            },
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White
            }
        };

        SetupDataGridViewColumns();
        this.Controls.Add(dgvMatches);
    }

    private void SetupDataGridViewColumns()
    {
        dgvMatches.Columns.Clear();

        dgvMatches.Columns.Add("Time", "Hour");
        dgvMatches.Columns.Add("Home", "HomeTeam");
        dgvMatches.Columns.Add("Away", "AwayTeam");
        dgvMatches.Columns.Add("Sport", "SportName");
        dgvMatches.Columns.Add("League", "LeagueName");
        dgvMatches.Columns.Add("Status","StatusMatch");
        dgvMatches.Columns.Add("Score", "ScoreMatch");

        dgvMatches.Columns["Time"].FillWeight = 15;
        dgvMatches.Columns["Home"].FillWeight = 20;
        dgvMatches.Columns["Away"].FillWeight = 20;
        dgvMatches.Columns["Sport"].FillWeight = 15;
        dgvMatches.Columns["League"].FillWeight = 20;
        dgvMatches.Columns["Status"].FillWeight = 15;
        dgvMatches.Columns["Score"].FillWeight = 15;
    }

    private void SetupStatusBar()
    {
        statusStrip = new StatusStrip
        {
            BackColor = Color.LightGray
        };

        var statusLabel = new ToolStripStatusLabel
        {
            Text = "Ready",
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft,
        };
        
        statusStrip.Items.Add(statusLabel);
        lblStatus = statusLabel;
        
        this.Controls.Add(statusStrip);
    }

    private void InitializeData()
    {
        teams = new List<Team>
        {
            // Football
            new Team { Id = 1, Name = "Real Madrid", Sport = "Football", League = "La Liga", Country = "Spain" },
            new Team { Id = 2, Name = "Barcelona", Sport = "Football", League = "La Liga", Country = "Spain" },
            new Team
            {
                Id = 3, Name = "Manchester United", Sport = "Football", League = "Premier League", Country = "England"
            },
            new Team { Id = 4, Name = "Chelsea", Sport = "Football", League = "Premier League", Country = "England" },
            new Team { Id = 5, Name = "Bayern Munich", Sport = "Football", League = "Bundesliga", Country = "Germany" },

            // Lol
            new Team { Id = 6, Name = "T1", Sport = "Lol", League = "LCK", Country = "South Korea" },
            new Team { Id = 7, Name = "G2 Esports", Sport = "Lol", League = "LEC", Country = "Europe" },
            new Team { Id = 8, Name = "Cloud9", Sport = "Lol", League = "LCS", Country = "North America" },
            new Team { Id = 9, Name = "Fnatic", Sport = "Lol", League = "LEC", Country = "Europe" },

            // CS2
            new Team { Id = 10, Name = "Astralis", Sport = "CS2", League = "ESL Pro League", Country = "Denmark" },
            new Team { Id = 11, Name = "NAVI", Sport = "CS2", League = "ESL Pro League", Country = "Ukraine" },
            new Team
            {
                Id = 12, Name = "FaZe Clan", Sport = "CS2", League = "ESL Pro League", Country = "International"
            },

            // Basketball
            new Team { Id = 13, Name = "Lakers", Sport = "Basketball", League = "NBA", Country = "USA" },
            new Team { Id = 14, Name = "Warriors", Sport = "Basketball", League = "NBA", Country = "USA" },

            // Valorant
            new Team { Id = 15, Name = "Sentinels", Sport = "Valorant", League = "VCT", Country = "USA" },
            new Team { Id = 16, Name = "Fnatic", Sport = "Valorant", League = "VCT", Country = "Europe" }
        };
        GenerateSampleMathes();
    }

    private void GenerateSampleMathes()
    {
        matches = new  List<Match>();
        var random = new Random();
        var statuses = new[] {"Schedule", "Live", "Finished", "Canceled"};

        int matchId = 1;
        var today = DateTime.Now;

        for (int day = -15; day <= 15; day++)
        {
            var matchDate = today.AddDays(day);
            var dailyMatches = random.Next(2, 8);

            for (int i = 0; i < dailyMatches; i++)
            {
                var sport = GetRandomSport();
                var sportTeams = teams.Where(t => t.Sport == sport).ToList();

                if (sportTeams.Count >= 2)
                {
                    var homeTeam = sportTeams[random.Next(sportTeams.Count)];
                    Team awayTeam;
                    do
                    {
                        awayTeam = sportTeams[random.Next(sportTeams.Count)];
                    } while (awayTeam.Id == homeTeam.Id);


                    var matchTime = matchDate.Date.AddHours(random.Next(8, 23)).AddMinutes(random.Next(0, 4) * 15);
                    var status = day < 0 ? "Finished" : (day == 0 && random.Next(10) < 2 ? "Live" : "Scheduled");

                    matches.Add(new Match
                    {
                        Id = matchId++,
                        HomeTeam = homeTeam.Name,
                        AwayTeam = awayTeam.Name,
                        Date = matchTime,
                        Sport = sport,
                        League = homeTeam.League,
                        Status = status,
                        Score = status == "Finished" ? $"{random.Next(0,5)}-{random.Next(0,5)}" :
                                status == "Live" ? $"{random.Next(0,3)}-{random.Next(0,3)}" : "",
                        Venue = GetRandomVenue(sport)
                    });
                }
            }
        }
    }

    private String GetRandomSport()
    {
        var sports = new[] { "Football", "Lol", "CS2", "Basketball", "Valorant", "Dota 2" };
        var weights = new[] { 40, 25, 15, 10, 5, 5 };

        var random = new Random();
        var totalWeight = weights.Sum();
        var randomNumber = random.Next(totalWeight);
        
        int currentWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];
            if (randomNumber < currentWeight)
            {
                return sports[i];
            }
        }

        return sports[0];
    }

    private string GetRandomVenue(string sport)
    {
        var venues = new Dictionary<string, string[]>
        {
            {"Football", new[] {"Santiago Bernabéu", "Camp Nou", "Old Trafford", "Stamford Bridge", "Allianz Arena"}},
            {"Lol", new[] {"LCK Arena", "LEC Studio", "LCS Studio", "Worlds Arena"}},
            {"CS2", new[] {"ESL Studio", "BLAST Arena", "IEM Cologne", "PGL Major Arena"}},
            {"Basketball", new[] {"Staples Center", "Madison Square Garden", "TD Garden", "American Airlines Center"}},
            {"Valorant", new[] {"VCT Stage", "Champions Arena", "Masters Berlin", "Reykjavik Studio"}}
        };

        if (venues.ContainsKey(sport))
        {
            var sportVenues = venues[sport];
            return sportVenues[new Random().Next(sportVenues.Length)];
        }

        return "TBA";
    }

    private void LoadMatches()
    {
        try
        {
            var filteredMatches = matches.AsEnumerable();

            if (cmbSports.SelectedItem?.ToString() != "All sports")
            {
                filteredMatches = filteredMatches.Where(m => m.Sport == cmbSports.SelectedItem.ToString());
            }

            filteredMatches = filteredMatches.Where(m => m.Date.Date == dtpDate.Value.Date);
            if (chkFavoriteOnly.Checked)
            {
                var favoriteTeams = teams.Where(t => t.IsFavorite).Select(t => t.Name).ToList();
                if (favoriteTeams.Any())
                {
                    filteredMatches = filteredMatches.Where(m =>
                        favoriteTeams.Contains(m.HomeTeam) || favoriteTeams.Contains(m.AwayTeam));
                }
                else
                {
                    filteredMatches = Enumerable.Empty<Match>();
                }
            }

            var matchList = filteredMatches.OrderBy(m => m.Date).ToList();

            DisplayMatches(matchList);

            var favoriteCount = teams.Count(t => t.IsFavorite);
            lblStatus.Text = $"Show {matchList.Count} match | team: {favoriteCount}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while loading: {ex.Message}", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    private void DisplayMatches(List<Match> matchList)
    {
        dgvMatches.Rows.Clear();
        foreach (var match in matchList)
        {
            var row = new DataGridViewRow();
            row.CreateCells(dgvMatches);
            
            row.Cells[0].Value = match.Date.ToString("HH:mm");
            row.Cells[1].Value = match.HomeTeam;
            row.Cells[2].Value = match.AwayTeam;
            row.Cells[3].Value = match.Sport;
            row.Cells[4].Value = match.League;
            row.Cells[5].Value = GetStatusDisplay(match.Status);
            row.Cells[6].Value = match.Score;

            var favoriteTeams = teams.Where(t => t.IsFavorite).Select(t => t.Name).ToList();
            if (favoriteTeams.Contains(match.HomeTeam) || favoriteTeams.Contains(match.AwayTeam))
            {
                row.DefaultCellStyle.BackColor = Color.LightYellow;
                row.DefaultCellStyle.Font = new Font(dgvMatches.Font, FontStyle.Bold);
            }

            switch (match.Status)
            {
                case "Live":
                    row.Cells[5].Style.BackColor = Color.LightGreen;
                    row.Cells[5].Style.ForeColor = Color.DarkGreen;
                    row.Cells[5].Style.Font = new Font(dgvMatches.Font, FontStyle.Bold);
                    break;
                case "Finished":
                    row.Cells[5].Style.BackColor = Color.LightGray;
                    break;
                case "Canceled":
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    break;
            }
            dgvMatches.Rows.Add(row);
        }
    }

    private string GetStatusDisplay(string status)
    {
        switch (status)
        {
            case "Scheduled": return "Coming soon";
            case "Live": return  "Live";
            case "Finished": return "Finished";
            case "Canceled": return "Canceled";
            default: return status;
        }
    }

    private void BtnManageFavorites_Click(object sender, EventArgs e)
    {
        var favoritesForm = new FavoritesForm(teams);
        if (favoritesForm.ShowDialog() == DialogResult.OK)
        {
            LoadMatches();
        }
    }

    #endregion
}