namespace ScheduleApp;
using System.Text;

public class FavoritesForm : Form
{
    private readonly List<Team> teams;
    private CheckedListBox clbTeams;
    private Button btnSelectAll;
    private Button btnDeselectAll;
    private Button btnOk;
    private Button btnCancel;
    private Label lblInfo;

    public FavoritesForm(List<Team> teams)
    {
        this.teams = teams ?? throw new ArgumentNullException(nameof(teams));
        InitializeComponent();
        LoadTeams();
    }

    private void InitializeComponent()
    {
        // Form setup
        this.Text = "Manage Favorites";
        this.ClientSize = new Size(500, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.White;
        this.Padding = new Padding(10);

        // Title label
        var lblTitle = new Label
        {
            Text = "Select your favorite teams",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Arial", 14, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Info label
        lblInfo = new Label
        {
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Arial", 9),
            ForeColor = Color.Gray,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Button panel
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 50,
            BackColor = Color.Transparent
        };

        // Buttons
        btnSelectAll = new Button
        {
            Text = "Select all",
            Size = new Size(100, 30),
            BackColor = Color.Green,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        btnSelectAll.Click += BtnSelectAll_Click;

        btnDeselectAll = new Button
        {
            Text = "Unselect all",
            Size = new Size(100, 30),
            Location = new Point(110, 0),
            BackColor = Color.Red,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        btnDeselectAll.Click += BtnDeselectAll_Click;

        btnOk = new Button
        {
            Text = "Save",
            Size = new Size(80, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Blue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Arial", 10, FontStyle.Bold),
            DialogResult = DialogResult.OK
        };
        btnOk.Click += BtnOk_Click;

        btnCancel = new Button
        {
            Text = "Cancel",
            Size = new Size(80, 30),
            Location = new Point(90, 0),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Arial", 10),
            DialogResult = DialogResult.Cancel
        };

        // CheckedListBox
        clbTeams = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            CheckOnClick = true,
            Font = new Font("Arial", 10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            IntegralHeight = false
        };

        // Layout
        buttonPanel.Controls.Add(btnSelectAll);
        buttonPanel.Controls.Add(btnDeselectAll);
        
        var okCancelPanel = new Panel
        {
            Dock = DockStyle.Right,
            Width = 180,
            BackColor = Color.Transparent
        };
        okCancelPanel.Controls.Add(btnCancel);
        okCancelPanel.Controls.Add(btnOk);
        buttonPanel.Controls.Add(okCancelPanel);

        this.Controls.Add(clbTeams);
        this.Controls.Add(buttonPanel);
        this.Controls.Add(lblInfo);
        this.Controls.Add(lblTitle);

        // Update info text
        UpdateSelectionInfo();
    }

    private void LoadTeams()
    {
        
        clbTeams.BeginUpdate();
        try
        {
            clbTeams.Items.Clear();
            var groupedTeams = teams
                .GroupBy(t => t.Sport)
                .OrderBy(g => GetSportOrder(g.Key));

            foreach (var group in groupedTeams)
            {
                // Add sport header
                clbTeams.Items.Add($"== {GetSportIcon(group.Key)} {group.Key.ToUpper()} ==", false);

                // Add teams
                foreach (var team in group.OrderBy(t => t.Name))
                {
                    var index = clbTeams.Items.Add(CreateTeamDisplayText(team), team.IsFavorite);
                    clbTeams.SetItemChecked(index, team.IsFavorite);
                }

                // Add spacer
                clbTeams.Items.Add("", false);
            }
        }
        finally
        {
            clbTeams.EndUpdate();
        }
    }
    
    private string CreateTeamDisplayText(Team team)
    {
        var sb = new StringBuilder();
        sb.Append($"   {GetTeamIcon(team.Sport)} {team.Name}");

        if (!string.IsNullOrEmpty(team.League))
        {
            sb.Append($" ({team.League})");
        }

        if (!string.IsNullOrEmpty(team.Country))
        {
            sb.Append($" - {team.Country}");
        }

        return sb.ToString();
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
            if (clbTeams.GetItemChecked(i) && !itemText.StartsWith("==") && !string.IsNullOrEmpty(itemText.Trim()))
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
        if (string.IsNullOrWhiteSpace(displayText))
            return string.Empty;

        // Remove leading spaces and icons
        var text = displayText.TrimStart();
        if (text.StartsWith("   "))
            text = text[3..].TrimStart();

        // Find the first of these delimiters
        var delimiters = new[] { " (", " - " };
        var firstDelimiterIndex = delimiters
            .Select(d => text.IndexOf(d, StringComparison.Ordinal))
            .Where(i => i > 0)
            .DefaultIfEmpty(-1)
            .Min();

        return firstDelimiterIndex > 0 
            ? text[..firstDelimiterIndex].Trim() 
            : text.Trim();
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