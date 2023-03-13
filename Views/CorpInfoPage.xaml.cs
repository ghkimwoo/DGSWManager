using System.Collections.ObjectModel;
namespace DGSWManager.Views;

public partial class CorpInfoPage : ContentPage
{
	public class Content
	{
		public string ContentName { get; set; }
		public string ContentDescription { get; set; }
	}
    ObservableCollection<Content> contents = new ObservableCollection<Content>();
    public ObservableCollection<Content> Contents { get { return contents; } }
    public CorpInfoPage()
	{
		InitializeComponent();
        contents.Add(new Content() { ContentName = "Apple", ContentDescription = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        contents.Add(new Content() { ContentName = "Orange", ContentDescription = "The orange is the fruit of various citrus species in the family Rutaceae." });
        contents.Add(new Content() { ContentName = "Banana", ContentDescription = "A long curved fruit with soft pulpy flesh and yellow skin when ripe." });
        contents.Add(new Content() { ContentName = "Grape", ContentDescription = "A berry growing in clusters on a grapevine, eaten as fruit and used in making wine." });
        contents.Add(new Content() { ContentName = "Mango", ContentDescription = "A mango is an edible stone fruit produced by the tropical tree Mangifera indica." });
        collectionView.ItemsSource = contents;
    }
}