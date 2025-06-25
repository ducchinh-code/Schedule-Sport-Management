namespace ScheduleApp;

public class FavoritesForm : Form
{
    private List<Team> teams;
    private CheckedListBox clbTeams;
    private Button btnSelectAll;
    private Button btnDeselectAll;
    private Button btnOk;
    private Button btnCancel;
    private Label lblInfo;
    private GroupBox grbFootball;
    private GroupBox grbEsports;
    private GroupBox grbOthers;

    public FavoritesForm(List<Team> teams)
    {
        this.teams = teams;
        InitializeComponent();
        LoadTeams();
    }

    private void InitializeComponent()
    {
        this.Text = "Manage Favorites";
        this.Size = new Size(500, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.White;

        var lblTitle = new Label()
        {
            Text = "Select your favorite team",
            Location = new Point(20, 20),
            Size = new Size(450, 25),
            Font = new Font("Arial", 14, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            TextAlign = ContentAlignment.MiddleCenter,
        };

        lblInfo = new Label()
        {
            Text = "Select your favorite teams. Your favorite teams's match will be highlighted",
            Location = new Point(20, 55),
            Size = new Size(450, 30),
            Font = new Font("Arial", 9),
            ForeColor = Color.Gray,
            TextAlign = ContentAlignment.MiddleCenter,
        };

        btnSelectAll = new Button()
        {
            Text = "Select all",
            Location = new Point(20, 95),
            Size = new Size(100, 30),
            BackColor = Color.Green,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
        };
        btnSelectAll.Click += BtnSelectAll_Click;

        btnDeselectAll = new Button()
        {
            Text = "Unselect all",
            Location = new Point(130, 95),
            Size = new Size(100, 30),
            BackColor = Color.Red,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
        };
        btnDeselectAll.Click += BtnDeselectAll_Click;

        clbTeams = new CheckedListBox()
        {
            Location = new Point(20, 140),
            Size = new Size(450, 350),
            CheckOnClick = true,
            Font = new Font("Arial", 10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        btnOk = new Button()
        {
            Text = "Save data",
            Location = new Point(280, 510),
            Size = new Size(120, 35),
            BackColor = Color.Blue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Arial", 10, FontStyle.Bold),
            DialogResult = DialogResult.OK
        };
        btnOk.Click += BtnOk_Click;

        btnCancel = new Button()
        {
            Text = "Cancel",
            Location = new Point(410, 510),
            Size = new Size(100, 35),
            BackColor = Color.Gray,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Arial", 10),
            DialogResult = DialogResult.Cancel
        };
        
        this.Controls.AddRange(new Control[]
        {
            lblTitle,lblInfo,btnSelectAll,btnDeselectAll,clbTeams,btnOk,btnCancel
        });
    }

    private void LoadTeams()
    {
        clbTeams.Items.Clear();
        var groupedTeams = teams.GroupBy(t => t.Sport).OrderBy(g => GetSportOrder(g.Key));

        foreach (var group in groupedTeams)
        {
            var header = $"== {GetSportIcon(group.Key)} {group.Key.ToUpper()} ==";
            clbTeams.Items.Add(header, false);

            foreach (var team in group.OrderBy(t => t.Name))
            {
                var displayText = $"   {GetTeamIcon(team.Sport)} {team.Name}";
                if (!string.IsNullOrEmpty(team.League))
                {
                    displayText += $" {team.League}";
                }

                if (!string.IsNullOrEmpty(team.Country))
                {
                    displayText += $" - {team.Country}";
                }
                clbTeams.Items.Add(displayText, team.IsFavorite);
            }

            clbTeams.Items.Add("", false);
        }

        UpdateSelectionInfo();
    }

    private int GetSportOrder(string sport)
    {
        switch (sport)
        {
            case "Football": return 1;
            case "Lol" : return 2;
            case "CS2" : return 3;
            case "Valorant" :  return 4;
            case "Basketball" : return 5;
            case "Dota 2" :  return 6;
            default: return 99;
        }
    }

    private string GetSportIcon(string sport)
    {
        switch (sport)
        {
            case "Football": return "⚽";
            case "Lol" : return "🎮";
            case "CS2" : return "🔫";
            case "Valorant" :  return "🎯";
            case "Basketball" : return "🏀";
            case "Dota 2" :  return "🛡️";
            default: return "🏆";
        }
    }

    private string GetTeamIcon(string team)
    {
        return "";
    }

    private void BtnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < clbTeams.Items.Count; i++)
        {
            var itemText = clbTeams.Items[i].ToString();
            if (!itemText.StartsWith("==") && !string.IsNullOrEmpty(itemText.Trim()))
            {
                clbTeams.SetItemChecked(i,true);
            }
        }
        UpdateSelectionInfo();
    }

    private void BtnDeselectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < clbTeams.Items.Count; i++)
        {
            clbTeams.SetItemChecked(i,false);
        }

        UpdateSelectionInfo();
    }

    private void UpdateSelectionInfo()
    {
        var selectedCount = 0;
        for (int i = 0; i < clbTeams.Items.Count; i++)
        {
            var itemText = clbTeams.Items[i].ToString();
            if (clbTeams.GetItemChecked(i) && !itemText.StartsWith("'==") && !string.IsNullOrEmpty(itemText.Trim()))
            {
                selectedCount++;
            }
        }

        lblInfo.Text = $"{selectedCount} teams selected, their match will be highlighted";
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < clbTeams.Items.Count; i++)
            {
                var itemText = clbTeams.Items[i].ToString();
                if (!itemText.StartsWith("'==") && !string.IsNullOrEmpty(itemText.Trim()))
                {
                    var teamName = ExtractTeamName(itemText);
                    var team = teams.FirstOrDefault(t => t.Name == teamName);
                    if (team != null)
                    {
                        team.IsFavorite = clbTeams.GetItemChecked(i);
                    }
                }
            }

            var selectedCount = teams.Count(t => t.IsFavorite);
            MessageBox.Show($"{selectedCount} favorite teams was updated",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error changed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string ExtractTeamName(string displayText)
    {
        var text = displayText.Trim();
        if (text.StartsWith("   "))
        {
            text = text.Substring(3).Trim();
        }

        if (text.Length > 2 && text[1] == ' ')
        {
            text = text.Substring(2).Trim();
        }
        
        var parentIndex = text.IndexOf('(');
        if (parentIndex > 0)
        {
            text = text.Substring(0,parentIndex).Trim();
        }
        var dashIndex = text.IndexOf(" - ");
        if (dashIndex > 0)
        {
            text = text.Substring(0, dashIndex).Trim();
        }
        return text;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        clbTeams.ItemCheck += (s, e) =>
        {
            this.BeginInvoke(new Action(() => UpdateSelectionInfo()));
        };
    }
}