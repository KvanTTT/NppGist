using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NppGist.JsonMapping;

namespace NppGist.Forms
{
    public class Paginator
    {
        private TreeView treeView;
        private TextBox pageNumberTextBox;
        private Button prevPageButton, nextPageButton;
        private bool showRoot;

        public Dictionary<string, Gist> Gists { get; private set; }

        public static int GistsPerPage = 30;

        public Paginator(TreeView treeView, Button prevPageButton, Button nextPageButton, TextBox pageNumberTextBox, bool showRoot)
        {
            this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
            this.prevPageButton = prevPageButton ?? throw new ArgumentNullException(nameof(prevPageButton));
            this.nextPageButton = nextPageButton ?? throw new ArgumentNullException(nameof(nextPageButton));
            this.pageNumberTextBox = pageNumberTextBox ?? throw new ArgumentNullException(nameof(pageNumberTextBox));
            this.showRoot = showRoot;
        }

        public async Task<bool> UpdateGists(PageStatus pageStatus)
        {
            try
            {
                int pageNumber = int.Parse(pageNumberTextBox.Text);

                if (pageStatus == PageStatus.Init)
                {
                    pageNumber = 1;
                }
                else if (pageStatus == PageStatus.Next)
                {
                    pageNumber++;
                }
                else if (pageStatus == PageStatus.Prev)
                {
                    pageNumber--;
                }

                var takenGists = await Main.GitHubService.SendJsonRequestAsync<List<Gist>>($"gists?page={pageNumber}&per_page={GistsPerPage}");
                var newGists = takenGists.ToDictionary(gist => gist.Id);

                bool prevPageButtonEnabled, nextPageButtonEnabled;
                if (newGists.Count > 0)
                {
                    prevPageButtonEnabled = pageNumber != 1;
                    nextPageButtonEnabled = newGists.Count >= GistsPerPage;
                }
                else
                {
                    if (pageNumber == 1)
                    {
                        prevPageButtonEnabled = false;
                        nextPageButtonEnabled= false;
                    }
                    else
                    {
                        // Do not update list
                        newGists = null;
                        pageNumber--;
                        prevPageButtonEnabled = pageNumber != 1;
                        nextPageButtonEnabled = false;
                    }
                }

                if (prevPageButton.IsHandleCreated)
                {
                    prevPageButton.Enabled = prevPageButtonEnabled;
                    nextPageButton.Enabled = nextPageButtonEnabled;
                    pageNumberTextBox.Text = pageNumber.ToString();
                }

                if (newGists != null)
                {
                    Gists = newGists;

                    if (treeView.IsHandleCreated)
                    {
                        GuiUtils.RebuildTreeView(treeView, newGists, showRoot);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to connect to api.github.com. Try to refresh.{Environment.NewLine}Error message: {ex.Message}");

                return false;
            }
        }
    }
}