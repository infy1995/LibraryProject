using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LibraryProject.Models;

namespace LibraryProject.DAL
{
    public class LibraryInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LibraryContext>
    {
        protected override void Seed(LibraryContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Obsługa"));

            var user = new ApplicationUser{UserName = "asd@o2.pl"};
            string password = "zaq1@WSX";

            userManager.Create(user, password);

            userManager.AddToRole(user.Id, "Obsługa");

            var authors = new List<Author>
            {
                new Author()
                {
                    ID = 1,
                    FirstName = "Dmitrij",
                    LastName = "Głuchowski"
                },
                new Author()
                {
                    ID = 2,
                    FirstName = "Kass",
                    LastName = "Morgan"
                },
                new Author()
                {
                    ID = 3,
                    FirstName = "Stephen",
                    LastName = "King"
                },
                new Author()
                {
                    ID = 4,
                    FirstName = "William",
                    LastName = "Shakespeare"
                },
                new Author()
                {
                    ID = 5,
                    FirstName = "Adam",
                    LastName = "Mickiewicz"
                },
                new Author()
                {
                    ID = 6,
                    FirstName = "John",
                    LastName = "Green"
                },
            };

            authors.ForEach(s => context.Author.Add(s));
            context.SaveChanges();

            var publishers = new List<Publisher>
            {
                new Publisher()
                {
                    ID = 1,
                    Name = "Insignis"
                },
                new Publisher()
                {
                    ID = 2,
                    Name = "Bukowy Las"
                },
                new Publisher()
                {
                    ID = 3,
                    Name = "Prószyński i S-ka"
                },
                new Publisher()
                {
                    ID = 4,
                    Name = "Zielona Sowa"
                },
                new Publisher()
                {
                    ID = 5,
                    Name = "Greg"
                }
            };

            publishers.ForEach(s => context.Publishers.Add(s));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category()
                {
                    ID = 1,
                    Name = "Fantasy"
                },
                new Category()
                {
                    ID = 2,
                    Name = "Science Fiction"
                },
                new Category()
                {
                    ID = 3,
                    Name = "Horror"
                },
                new Category()
                {
                    ID = 4,
                    Name = "Klasyka"
                },
                new Category()
                {
                    ID = 5,
                    Name = "Kryminał"
                },
                new Category()
                {
                    ID = 6,
                    Name = "Sensacja"
                },
                new Category()
                {
                    ID = 7,
                    Name = "Thriller"
                },
                new Category()
                {
                    ID = 8,
                    Name = "Literatura Młodzieżowa"
                },
                new Category()
                {
                    ID = 9,
                    Name = "Literatura Obyczajowa"
                },
                new Category()
                {
                    ID = 10,
                    Name = "Romans"
                },
                new Category()
                {
                    ID = 11,
                    Name = "Literatura Piękna"
                },
                new Category()
                {
                    ID = 12,
                    Name = "Powieść historyczna"
                },
                new Category()
                {
                    ID = 13,
                    Name = "Powieść przygodowa"
                }
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var books = new List<Book>
            {
                new Book()
                {
                    ID = 1,
                    Isbn = "9788366071308",
                    Title = "Metro 2033",
                    Description = "Czy kiedykolwiek przyszło ci do głowy, że ostatni epizod historii cywilizacji człowieka rozegra się w przejmującej atmosferze moskiewskiego metra? Czy człowiek, który w marzeniach sięgał gwiazd, godzien jest skończyć jak szczur, sto metrów pod ziemią? Mało prawdopodobne? Po przeczytaniu „Metra 2033” – rosyjskiego, kultowego już bestsellera, który zawładnął wyobraźnią ponad dwuipółmilionowej rzeszy czytelników – na pewno zmienisz zdanie.\r\n\r\nTen thriller SF, którego wartka akcja i niezwykle sugestywnie przedstawiony świat nie pozwolą ci się od niego oderwać aż do ostatniej strony, to nie tylko wspaniała rozrywka i uczta dla czytelnika. To także portret człowieka u schyłku cywilizacji; przejmująca analiza jego niezmiennej natury. To obraz świata po końcu świata.\r\n\r\nRok 2033. W wyniku konfliktu atomowego świat uległ zagładzie. Ocaleli tylko nieliczni, chroniący się w moskiewskim metrze, które dzięki unikalnej konstrukcji stało się najprawdopodobniej ostatnim przyczółkiem ludzkości. Na mrocznych stacjach, rozświetlanych światłami awaryjnymi i blaskiem ognisk, ludzie ci próbują wieść życie zbliżone do tego sprzed katastrofy. Tworzą mikropaństwa spajane ideologią, religią czy po prostu ochroną filtrów wodnych... Zawierają sojusze, toczą wojny.\r\n\r\nWOGN to wysunięta najbardziej na północ zamieszkała stacja metra. Kiedyś była jedną z najpiękniejszych, a po zagładzie przez długi czas pozostawała bezpieczna. Teraz pojawiło się na niej śmiertelne niebezpieczeństwo.\r\n\r\nArtem, młody mężczyzna z WOGN-u, otrzymuje zadanie: musi przedostać się do legendarnej stacji Polis, serca moskiewskiego metra, aby przekazać ostrzeżenie o nowym niebezpieczeństwie. Od powodzenia jego misji zależy przyszłość nie tylko peryferyjnej stacji, ale być może całej ocalałej w metrze ludzkości.",
                    ReleaseDate = 2019,
                    Quantity = 10,
                    Author = authors[0],
                    Publisher = publishers[0],
                    Categories = new List<Category> {categories[0],categories[1]},
                    Image = "metro2033.jpg"
                },
                new Book()
                {
                    ID = 2,
                    Isbn = "9788364431846",
                    Title = "Misja 100",
                    Description = "Trzysta lat po spustoszeniu Ziemi przez kataklizm, który uczynił ją niezdatną do życia, ludzie wciąż przebywają w oddalonej od macierzystej planety stacji kosmicznej. Ściśle egzekwowana kontrola narodzin pozwala im oszczędzać kurczące się zasoby, a dorośli skazani za przestępstwa zostają natychmiast straceni. W obawie, że kolonii pozostało niewiele czasu, Kanclerz decyduje o wysłaniu setki młodocianych przestępców na Ziemię, żeby przekonać się, czy powierzchnia planety nadaje się do ponownego zamieszkania. Czy będzie to dla nich szansa na nowe życie, czy wręcz przeciwnie – pewna śmierć? W zetknięciu z dziką przyrodą setka zesłańców, prześladowana przez tajemnice z przeszłości, musi walczyć o życie. Nikt nie uważał ich za bohaterów, jednak dla całej ludzkości są ostatnią nadzieją na przetrwanie.",
                    ReleaseDate = 2015,
                    Quantity = 10,
                    Author = authors[1],
                    Publisher = publishers[1],
                    Categories = new List<Category> {categories[0],categories[1]},
                    Image = "100.jpg"
                },
                new Book()
                {
                    ID = 3,
                    Isbn = "9788366071315",
                    Title = "Metro 2034",
                    Description = "Rok 2034. W ostatnim schronieniu ludzkości pojawia się znów zagrożenie. Czy rok po starciu z Czarnymi ponownie uda się wygrać walkę o przetrwanie?\r\n\r\nMoskwa 2034. Od pamiętnych wydarzeń, które początek i finał miały na stacji WOGN, minął niespełna rok. Na drugim krańcu moskiewskiego metra mieszkańcy Sewastopolskiej toczą walkę o przetrwanie z nowymi formami życia, wdzierającymi się z powierzchni do ostatniej ludzkiej enklawy. Los stacji i jej mieszkańców zależy od dostaw amunicji, a te nagle ustają. Karawany giną bez wieści, urywa się łączność.\r\n\r\nZ zadaniem wyjaśnienia zagadki i przywrócenia dostaw wyruszają: młody Ahmed, leciwy, niespełniony kronikarz metra Homer i Hunter, który niegdyś zaginął wśród czarnych, a teraz odnalazł się, choć jego tożsamość budzi wątpliwość... Do wyprawy dołącza Sasza, córka wygnanego naczelnika Awtozawodzkiej.\r\n\r\nKim naprawdę jest Hunter? Czy odwzajemni uczucie, jakim obdarzyła go Sasza? Jaką tajemnicę skrywają mroczne tunele moskiewskiego metra? Czy garstce śmiałków uda się ocalić tych nielicznych, którzy przetrwali zagładę?\r\n\r\nNowe wydanie drugiej części trylogii „Metro” Dmitrija Glukhovsky'ego, podobnie jak pierwszy tom oraz premierowa powieść kończąca cykl, zyskał nową oprawę graficzną autorstwa uznanego ilustratora rosyjskiego Iliji Jackiewicza, twórcy okładek m.in. takich książek jak „Futu.re” (Glukhovsky), „Łzy Diabła” (Magdalena Kozak), „Szczury Wrocławia” (Robert J. Szmidt). Ilustracje zamieszczone w nowych wydaniach świetnie podkreślają postapokaliptyczny klimat powieści.",
                    ReleaseDate = 2018,
                    Quantity = 10,
                    Author = authors[0],
                    Publisher = publishers[0],
                    Categories = new List<Category> {categories[0],categories[1]},
                    Image = "metro2034.jpg"
                },
                new Book()
                {
                    ID = 4,
                    Isbn = "9788365315052",
                    Title = "Metro 2035",
                    Description = "Miejsce człowieka nie jest pod ziemią. Żyjecie w tunelach jak robaki! Tu nie ma dla nas jutra. Metro to cmentarz. Nie będziemy tu ludźmi. Nie stworzymy niczego nowego. Nie rozwiniemy się. Chorujemy tu. Wyradzamy się. Nie ma powietrza. Nie ma miejsca. Jest ciasno.\r\n\r\nTrzecia wojna światowa starła ludzkość z powierzchni Ziemi. Planeta opustoszała. Całe miasta obróciły się w proch i pył. Przestał istnieć transport, zamarła komunikacja. Radio milczy na wszystkich częstotliwościach. W Moskwie przeżyli tylko ci, którzy przy wtórze syren alarmowych zdążyli dobiec do bram metra. Tam, na głębokości dziesiątek metrów, na stacjach i w tunelach, ludzie próbują przeczekać koniec cywilizacji. W miejsce utraconego ogromnego świata stworzyli swój własny ułomny światek. Czepiają się życia i ani myślą się poddać. Pewnie marzą o powrocie na powierzchnię – kiedyś, kiedy obniży się poziom radiacji. I nie tracą nadziei na odnalezienie innych ocalałych…\r\n\r\nMetro 2035 kontynuuje historię Artema z pierwszego tomu kultowej serii. Na tę książkę miliony czekały przez całe dziesięć lat, a prawa do tłumaczenia wydawnictwa wykupiły na długo przed jej ukończeniem. Metro 2035 jest przy tym książką niezależną i również od niej można zacząć przygodę z cyklem Glukhovsky’ego, który podbił serca czytelników w Rosji i na całym świecie.\r\n\r\n„Zwyczajny i znajomy świat Metra postawiłem na głowie, tak więc tych, którzy czytali »Metro 2033« czeka mnóstwo odkryć i niespodzianek. A tym, którzy swój kontakt z »Metrem« zaczynają od tej właśnie książki, oddaję sensacyjną, emocjonalną, mocną powieść – myślę, że nie pozwoli im się nudzić”.\r\n– Dmitry Glukhovsky",
                    ReleaseDate = 2018,
                    Quantity = 10,
                    Author = authors[0],
                    Publisher = publishers[0],
                    Categories = new List<Category> {categories[0],categories[1]},
                    Image = "metro2035.jpg"
                },
                new Book()
                {
                    ID = 5,
                    Isbn = "9788380973114",
                    Title = "Carrie",
                    Description = "Carrie White jest inna niż jej rówieśnicy. Nie chodzi na prywatki, nie interesują jej chłopcy, stanowi obiekt kpin i docinków. Matka - religijna fanatyczka - za wszelką cenę usiłuje uchronić ją przed grzechem. Pewnego razu Carrie buntuje się i idzie na szkolny bal. Gdy pada tam ofiarą okrutnego żartu, rozpętuje się piekło...",
                    ReleaseDate = 2017,
                    Quantity = 10,
                    Author = authors[2],
                    Publisher = publishers[2],
                    Categories = new List<Category> {categories[2]},
                    Image = "carrie.jpg"
                },
                new Book()
                {
                    ID = 6,
                    Isbn = "9788381690584",
                    Title = "Smętarz dla zwierzaków",
                    Description = "Czasami martwe jest lepsze.\r\nZazwyczaj przeprowadzka to początek nowego życia, ale dla rodziny Creedów stała się początkiem ich końca. Mistrz horroru Stephen King zaprasza czytelników na wycieczkę do piekła i z powrotem!\r\n\r\nNa świecie istnieją dobre i złe miejsca. Nowy dom rodziny Creedów w Ludlow był niewątpliwie dobrym miejscem - przytulną, przyjazną wiejską przystanią po zgiełku i chaosie Chicago. Cudowne otoczenie Nowej Anglii, łąki, las to idealna siedziba dla młodego lekarza, jego żony, dwójki dzieci i kota. Wspaniała praca, mili sąsiedzi i droga, po której nieustannie przetaczają się ciężarówki. Droga i miejsce za domem, w lesie, pełne wzniesionych dziecięcymi rękami nagrobków - to tam dzieci z miasteczka zakopują swe martwe zwierzaki. Ci, którzy nie znają przeszłości, zwykle ją powtarzają... i nie chcą słuchać ostrzeżeń.\r\n\r\nStephen King napisał tę powieść podczas pobytu w wynajętym domku w Orrington, w stanie Maine. Fakt ten niewiele by znaczył, gdyby nie cmentarz zwierząt, który znajdował się nieopodal…\r\n\r\nNajnowsza ekranizacja bestsellerowej powieści mistrza grozy trafi do polskich kin już 3 maja 2019 roku.\r\nW filmie zobaczymy m.in. Jasona Clarke’a, Johna Lithgowa oraz Amy Seimetz.",
                    ReleaseDate = 2019,
                    Quantity = 10,
                    Author = authors[2],
                    Publisher = publishers[2],
                    Categories = new List<Category> {categories[2]},
                    Image = "smetarz.jpg"
                },
                new Book()
                {
                    ID = 7,
                    Isbn = "9788380731882",
                    Title = "Romeo i Julia",
                    Description = "Werona. Monteki i Kapulet, dwa od wieków zwaśnione rody i historia pięknej, romantycznej miłości, która nigdy nie powinna się wydarzyć… Miłości Romea i Julii – młodych kochanków pochodzących z obydwu rodzin.\r\nŚlub, który jest potwierdzeniem wiecznego uczucia zakochanych, ma wróżyć szczęśliwe zakończenie. Karta losu niestety odwraca się i na skutek błędnego toku zdarzeń zmierza ku tragedii, której nie będzie można już powstrzymać...\r\n\r\nSeria „Literatura Klasyczna” to zbiór najpiękniejszych dzieł światowej literatury dla dzieci i młodzieży. Piękne, nowoczesne wydanie sprawia, że czytelnicy w każdym wieku z przyjemnością sięgną po lekturę, a piękne ilustracje umilą czytanie i pomogą wyobraźni przenieść się do świata fascynujących historii.",
                    ReleaseDate = 2017,
                    Quantity = 10,
                    Author = authors[3],
                    Publisher = publishers[3],
                    Categories = new List<Category> {categories[3]},
                    Image = "romeo.jpg"
                },
                new Book()
                {
                    ID = 8,
                    Isbn = "9788375178302",
                    Title = "Dziady",
                    Description = "Dziady Adama Mickiewicza to jeden z najbardziej znanych utworów w literaturze polskiej i jednocześnie jedno z dzieł najbardziej reprezentatywnych dla polskiego romantyzmu. Od lat regularnie wystawiane na deskach teatrów całej Polski, czytane przez kolejne pokolenia czytelników, inspirujące twórców i niosące wciąż aktualne treści. Każda z części utworu porusza inne tematy, ale razem tworzą spójną wizję świata rządzonego przez niezmienne prawa boskiej sprawiedliwości i miłosierdzia.\r\n\r\nWydanie w serii Kolorowa Klasyka przygotowano ze szczególną dbałością o estetykę - kanoniczne XIX-wieczne ilustracje podkreślają atmosferę utworu, a doskonała okładka autorstwa znanego grafika Roberta Konrada przyciągnie oko każdego czytelnika.",
                    ReleaseDate = 2018,
                    Quantity = 10,
                    Author = authors[4],
                    Publisher = publishers[4],
                    Categories = new List<Category> {categories[3]},
                    Image = "dziady.jpg"
                },
                new Book()
                {
                    ID = 9,
                    Isbn = "9788364481178",
                    Title = "Gwiazd naszych wina",
                    Description = "Hazel choruje na raka i mimo cudownej terapii dającej perspektywę kilku lat więcej, wydaje się, że ostatni rozdział jej życia został spisany już podczas stawiania diagnozy. Lecz gdy na spotkaniu grupy wsparcia bohaterka powieści poznaje niezwykłego młodzieńca Augustusa Watersa, następuje nagły zwrot akcji i okazuje się, że jej historia być może zostanie napisana całkowicie na nowo.\r\n\r\nWnikliwa, odważna, humorystyczna i ostra książka to najambitniejsza i najbardziej wzruszająca powieść Johna Greena, zdobywcy wielu nagród literackich. Autor w błyskotliwy sposób zgłębia ekscytującą, zabawną, a równocześnie tragiczną kwestię życia i miłości.",
                    ReleaseDate = 2014,
                    Quantity = 10,
                    Author = authors[5],
                    Publisher = publishers[1],
                    Categories = new List<Category> {categories[7]},
                    Image = "gwiazd.jpg"
                },
            };
            books.ForEach(s => context.Books.Add(s));
            context.SaveChanges();
            var profile = new List<Profile>
            {
                new Profile
                {
                    ID = 1,
                    Login = "asd@o2.pl"
                }
            };
            profile.ForEach(s=> context.Profiles.Add(s));
        }
    }
}