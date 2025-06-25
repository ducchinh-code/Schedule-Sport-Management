namespace ScheduleApp;

public partial class MainForm : Form
{
    private DataGridView dgvMatches;
    private ComboBox cmbSports;
    private DateTimePicker dtpDate;
    private CheckBox chkFavoriteOnly;
    private Button btnManageFavorites;
    private Button btnRefresh;
    private ToolStripStatusLabel lblStatus;
    private Label lblTitle;
    private ToolStrip toolStrip;
    private StatusStrip statusStrip;

    private List<Match> matches;
    private List<Team> teams;
    
    public MainForm()
    {
        InitializeComponent();
        InitializeData();
        LoadMatches();
    }
}