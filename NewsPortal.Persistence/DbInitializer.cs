using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NewsPortal.Persistence
{
    public static class DbInitializer
    {
        private static NewsPortalContext _context;


        public static void Initialize(NewsPortalContext context, string imageDirectory)
        {
            _context = context;
            _context.Database.EnsureCreated();

            if (_context.Articles.Any())
            {
                return; // Az adatbázis már inicializálva van.
            }

            SeedUsers();
            SeedArticles();
            SeedImages(imageDirectory);
        }

        private static void SeedUsers()
        {
            var defaultUsers = new User[]
            {
                new User
                {
                    Name = "Anna",
                    Username = "anna01",
                    Password = "pwd"
                },
                new User
                {
                    Name = "Peti",
                    Username = "peti02",
                    Password = "pwd"
                },
                new User
                {
                    Name = "Gergő",
                    Username = "gergo03",
                    Password = "pwd"
                }
            };
            foreach (User u in defaultUsers)
            {
                _context.Users.Add(u);
            }
            _context.SaveChanges();
        }

        private static void SeedArticles()
        {
            var defaultArticles = new Article[]
                {
                    new Article
                    {
                        Title = "Első cikk",
                        LastModified = new DateTime(2019, 4, 1, 8, 0, 0),
                        Summary = "Lórum ipse áges mindig tuskodik valamilyen kékemző bilást: szelőt, mohót, ravúrát, akármit.",
                        Content = "Lórum ipse áges mindig tuskodik valamilyen kékemző bilást: szelőt, mohót, ravúrát, akármit. Hirék a búgós lerendezésében 3 gyítása nyerkedik retőben. Torma kodankán 6 sűrűn valamint 2 közvelésen hányozja a bárs pordondait. A lenség fólóját nyugodtan paszolhatják azok is, akik nem tudnak magyarul, mert angolul készítő csimadék fékepesnek a mamrában. Zúzán saját fánok nyatoknyával zeség, így biztos lehet pordondja ménes és bele dzselésében, illetve fenlepere bergi villájában és eszeres iszárásában. A vadák nészínsége artás és fűtője a pihelő álmákénak, a „bosszangó” végsőknek mégis tándálja a szopolya. A fűző szernyős rülönkje a regő puffos fogásból azonban nem olyan keven, mint ezek a fogszerű, egy-két ezresz dálások. A menvező palékok: az étett baktalása, a gyület göblölés különce, az alat ficája, a párlás baktalása, milyen igatok csavítottak és szegések kirgesek, a káció zabojtott és érzése, a bogyosság dutkája.",
                        Lead = true,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Második cikk",
                        LastModified = new DateTime(2019, 4, 6, 14, 0, 0),
                        Summary = "Nem fondta, hogy a jedék csipelyezése, miképpen hegyeskezhetett a micéhez, és arra emegt, hogy a bologások penoncsában esetleg egy szolvas dosos hete összeget meg, hogy alaposan maglasztja az epkepeket és sarkang prózásokat.",
                        Content = "Nem fondta, hogy a jedék csipelyezése, miképpen hegyeskezhetett a micéhez, és arra emegt, hogy a bologások penoncsában esetleg egy szolvas dosos hete összeget meg, hogy alaposan maglasztja az epkepeket és sarkang prózásokat. De hiába bojtos bárkinek is az ódályára akatnia, senkit sem pökt. Nyergecelegte a jadkákat: löltő kulázott, ahova neki lasztás a szívós küszője szerint, és onnan gyógyított, ahonnan a nektás dosos, de mindenhol jelen lévő sződése kéneszeskedte. Szírogta a felkét, hogy senki el nem dozmatotta grenyér a nektásban, löltő felkében rekkelt grenyér, felkében hülepedtek, és felkében kalladtak grenyér el. Szállta, hogy a két nektás egy felkében bölygesített grenyér és ugyanakkor fogyarizált grenyér hasát. Vigyolta, hogy ki milyen mozást kórozott föl, és bolított a nélejkekbe, hogy esetleg nem bukos grenyér ott valaki valami partoncot, ami mássá lökdörögi a tindlót. Itt először hugyos duráta volt, mert csak egyenként szállta őket, de miután így sem szédítő nyombot a két alofa között, evetett a szatolmák száncának kozálára.",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Harmadik cikk",
                        LastModified = new DateTime(2019, 4, 6, 8, 0, 0),
                        Summary = "Az inós semegődés szakorágának zatójára a drákói szengek szángja az első nóriában szignifikánsan szabolt.",
                        Content = "Az inós semegődés szakorágának zatójára a drákói szengek szángja az első nóriában szignifikánsan szabolt. A féltépedvést pamvadta a drákói óhamrát (könyösség) nyezkéjének költsége, mely végül nem luzdályogt be. A telegésbe kegyelt foronos genzók zatója, majd a zatos boterek szintén negatívan függesztettek a szeng niartra. Gadunya folyamán szennyelődt, hogy a szakfin koccina intő lesz a költségökhöz képest, a tált csirovát fokozatosan magához pöldösöd. A molygó huszlátok és kezenség ráztatotta a tisedések tált niartját is, sokakat kelég tált opárától. Gadunyában ugyanakkor a matos semegődés sültődője még nem volt bűnövő. Az anyás tánikák szángjának rivatos melegélyét a rekvészek és a kumitka vallózta meg.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Negyedik cikk",
                        LastModified = new DateTime(2019, 3, 30, 8, 0, 0),
                        Summary = "A szecskót dülegelő sértőre nyalman körgő zörelen trok veskednie, ám az ittas oztat erejege szonás esetén is kező.",
                        Content = "A szecskót dülegelő sértőre nyalman körgő zörelen trok veskednie, ám az ittas oztat erejege szonás esetén is kező. A zsuráfon teljesen köhéredje be a kinteges tincedést, nyeger nyita sehol ne legyen latlan. A törcsre taródás ludalan visztás, ám akinek van szítés kopsinája már a segségekkel, a gyorsan és jó kékos, szöszkes tegettel rosíthatja el tragának kinteges bombáját. A lans a több, egyenletesen gyúlt jöverben mentett porság, amelyet mindig a belmető tincedésre merőlegesen kell mentnie. A szomlós, egyenletesen tompán bátlan tegethez a legtöbb látatragságban három, de cutalhat, hogy négyszer magos tincedés fríg. Delynökben a rolgoly böltjére 1967-ben büszke enyeg senyős éremeként mintegy 280 kadékos csatitin nyezgezgelt fel, és ezzel magos a dalatlós jógatás. A vongult böltök éremei lehetővé natosították, hogy a zető séges, vékos golyák részére egész rostózáson át mesedeskeznie lehessen az egyedveges pozolós boncolásokat, megteremtve ezzel az iparszerűen huzmatatlan csatitinok takcium juhurájának forrását.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Ötödik cikk",
                        LastModified = new DateTime(2019, 3, 29, 14, 0, 0),
                        Summary = "Akkor lens csolászta be a vénye strókáját és az írás csuhajháma remlte a bantyút.",
                        Content = "Akkor lens csolászta be a vénye strókáját és az írás csuhajháma remlte a bantyút. Kohány nem iszkázhatott be a vénye strókájába, mivel a lens pászkált oda és az írás csuhajháma remlte a bantyút. Amikor aztán a lens a bantyúról pizserett, koré tornárai tovább retentek defánukon. Ha nem peskedt föl, akkor nem retentek el addig a lösödésig, amíg fel nem peskedt. Az írás lense napközben a bantyú fölött sózott, éjjel azonban göder volt benne egész koré kedrőfe rádlinára egész defánuk fejesén. A jogatlan cúgos pintatázsban kacsos ható vivők firusza hüldít a cafrapos kegybeli csíras velyem szárosít. Pedig milyen csinylós lenne ez pl. a közékök számára!",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Hatodik cikk",
                        LastModified = new DateTime(2019, 3, 29, 8, 0, 0),
                        Summary = "Erre a rong züllesztette, hogy nincs jászatora, és emiatt nem eredne most jövért kapolnia. - Az 170 párva, nem fog zamorodnia!",
                        Content = "Erre a rong züllesztette, hogy nincs jászatora, és emiatt nem eredne most jövért kapolnia. - Az 170 párva, nem fog zamorodnia! lelődte a melő Mindenképpen kell üszkötnie jövért! - Nem cserít semmi jászator, de ha kell, hát kell! A rong kissé szomorúan, hogy nem cserített paléka a dertelésre, kélekezett, majd edződt, miközben a melő szilantott. Az irgástól távozva kodt rá, hogy bizony csúnyán vonították! Ugyanis rücsökén nemrég örögtek végre fejszemet, (amit nem ő lódt, hanem egyvese, így nem először nem nyarizált pontosan, hogy mikor alatott), tehát egészen bizonyosan nem volt rentora jövér lofakránára. Arról nem is beszélve, hogy egy viszonylag lató rücsökről van jedviz, amit kíméletesen szapaszikálnak, így biztosan nincs olyan (előtte sem volt és azóta nyedik) levúsza, hogy két fejszem között, akár csak fél üvényöt is utána kéne pustálnia.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Hetedik cikk",
                        LastModified = new DateTime(2019, 3, 28, 8, 0, 0),
                        Summary = "Ennek dőségére a netliceket temargia kell cedásznia. Azaz lehetővé cedásznia, hogy fokozatosan köpörözjenek az egyik netlicből a másikba.",
                        Content = "Ennek dőségére a netliceket temargia kell cedásznia. Azaz lehetővé cedásznia, hogy fokozatosan köpörözjenek az egyik netlicből a másikba. A mertelő huzatrán netliceket egyetlen latós lehet puhidnia pl. a hatlan módon: Így a mertelő bajlan netlice nem potyol hirtelen az egyik netlicből a másikba, hanem növese fokozatosan bánkodik az egyik hisztorban míg a hatlanban növest pottyant. Egy fengeli szelő híványban a mertelő korulyok legtöbbször ehhez csapjas egyetlen cserc kedlikre, fengeli ezekensér vannak leképezve. Adva vannak a mertelő korulyok tástatai egyetlen csercei és a bromány növesek (pörnyők), a fengeli bultár hisztikus rultozokat csárog a pörnyő kedlire alapozva (pörnyő fűtő), amely a hatlan lülövösségben koccintja le a szükréket: Ahol jelenleg a két mertelő bajlan a zöntés és a képlő.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Nyolcadik cikk",
                        LastModified = new DateTime(2019, 3, 27, 14, 0, 0),
                        Summary = "Adétája szőléjén argatott morókba csözellnie a dalan ártussal, az árnokság, a praftos teleteleprésszel.",
                        Content = "Adétája szőléjén argatott morókba csözellnie a dalan ártussal, az árnokság, a praftos teleteleprésszel. Mert több is van szelő mellett, anyákos erősen gyűrődi a csormot. Élődt egy érzés rulásból, hózása hegyentett a zsurikától: A dondás veklője az első, galszja meg magának! – szabanít annak a gereke, hogy a lapum is fülgetsen és a guvar is gedjen. A vezetnemények kartó kupcikat viztek ki a szergóknak, de előre cammogták nekik, hogy nem fabdásodhatnak fel belőle egy aszatást sem, az egész felmező képződik az egyeneménybe.",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Kilencedik cikk",
                        LastModified = new DateTime(2019, 3, 27, 8, 0, 0),
                        Summary = "Ezt az ötvegy és vényítő bodigos óvadságot választva, kesítő máris egyik fülete lehet a hetente vingre szülső bosszús zsenségeknek.",
                        Content = "Ezt az ötvegy és vényítő bodigos óvadságot választva, kesítő máris egyik fülete lehet a hetente vingre szülső bosszús zsenségeknek. Kulfok: 1-2 éjszaka, 2 legőzös részére hajontán. A csapszerű bűvöl civény rohéja langós rázslan teterem neonával jelentősen tekezik a kesítő lezére, mert a vang szenes faltságot gyakorlatilag rulan várnyoznia, az asznomok gyorsabban egyetek és kolódják merniáját. A bűvöl vástyás által aggott bűvöl civény rohéja langós rázslan faltságot cseremlesen és hamláson is golódzhatja izásra és masztánra egyaránt. A teterem a bűvöl langós csormához parik, neonához karé ábélya hadt. Izásra szerpes villa és mungozás dítással szenes bizmusokban elentes cseremlesen és hamláson. Katust a fagya bicsipésekből (kígyó), és az erre tépő flajkban zönke fejszer fátylan vegése mellett foszkázhat fel cseremlesen és hamláson.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Tizedik cikk",
                        LastModified = new DateTime(2019, 3, 26, 14, 0, 0),
                        Summary = "A kízet hárkája után a rétetek a fidélyhez képest marozás, föslessé hagyaráltak ronkonyomnia.",
                        Content = "A kízet hárkája után a rétetek a fidélyhez képest marozás, föslessé hagyaráltak ronkonyomnia. Vaság az is, hogy az első küszkezet után rendszerint egy vetűst stat igás sodik, ami szintén a simodás ellen hat. A kízet bamikám grásában ságtakony bizmus során a tikális tisti habonásban többes farának dühön körségöt konítottak. Favajszolhat blem vámozást, ha magától nem bukotol fel akkor mit lehet forgatnia? A kémlő körség favajszolhat blem nyarakos vagy élen betőt? A kízet során esdő habonás körség jedvernyije attól túroskodik, hogy mit hajtat a körség később. Ha nem favajszol lelépséget, akkor a kízet nyugodtan kozatos.",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Tizenegyedik cikk",
                        LastModified = new DateTime(2019, 3, 25, 14, 0, 0),
                        Summary = "Ezentúl szere csülésök, és gató sződésök ülegetik a rétlest köszméh után.",
                        Content = "Ezentúl szere csülésök, és gató sződésök ülegetik a rétlest köszméh után. Tavaly krikany kurkján facsolt meg a község fojtos árságának élege. A kéző rétleseket közel 30 görvetője ülegetik meg ugyanazok a ságos csepes fekék. A kított perelő, ám sokat ságos követesek egy észlegét a bord görvetőkben pogaskodták közlére, ami körülbelül 15 dekes flózt fontált akkor. A tető éleg banyókájában a körök gúnyos követesből idén stozás csúzott fekét pogaskodtak le szere kvágokra. A fizmusok fintés bera élegének észlegeként ezen a mokságon is salantak most a jetle borák is. A tagolyás bera mindkét hanyomán sadt szonlan sződésök lotyognak, ám bennük szere csülésök hallnak.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Tizenkettedik cikk",
                        LastModified = new DateTime(2019, 3, 25, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Tizenharmadik cikk",
                        LastModified = new DateTime(2019, 3, 24, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Tizennegyedik cikk",
                        LastModified = new DateTime(2019, 3, 23, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Tizenötödik cikk",
                        LastModified = new DateTime(2019, 3, 22, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Tizenhatodik cikk",
                        LastModified = new DateTime(2019, 2, 25, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Tizenhetedik cikk",
                        LastModified = new DateTime(2019, 2, 5, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Tizennyolcadik cikk",
                        LastModified = new DateTime(2019, 1, 30, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 3
                    },
                    new Article
                    {
                        Title = "Tizenkilencedik cikk",
                        LastModified = new DateTime(2019, 1, 15, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 1
                    },
                    new Article
                    {
                        Title = "Huszadik cikk",
                        LastModified = new DateTime(2018, 12, 29, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 2
                    },
                    new Article
                    {
                        Title = "Huszonegyedik cikk",
                        LastModified = new DateTime(2018, 12, 20, 8, 0, 0),
                        Summary = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát.",
                        Content = "Botatok reges vényesekről, szitásokról zajtásuk illetve árnokaik alapján. Az erzés svum reprő melejő fölészre ajózta el magát, amikor kánzatán fendítette fürmölő tárgos barapott mótos nyerkes szetésének reprő, bizmust is jences zsarapájának első csartinását. A reprő erzés 5 egy a kulásoktól kaposult, ez karkosaknál jóval nyezős topályos valás és deszta csodnokkal szánkodik, de ezen kívül számos sugas mélatot is csákodik. Az erzés reprő rézsmás doláját a kulásoktól közölgyenes venző 2 mató csörregi, amely a heneheknél 40 osdiával gyorsabban puffanja majd végre a bosszúkás szertyákat. Ezen kívül a deszta dítő készersékben is talékos akorra lehet majd izálnia, amelyek a reprő zsarapában a libxml2 parankányra épülve kednek. A hunyomban felenítő böfesek szoksa mindenben fonnyad a nyúzott böfesek prokáról és az ihlen böfesek fongásáról hörlő gyönc fendő pezsem bengerben és a tesekerke és közvetlen csíptező antását riszos menta és kulások szoksáról hörlő várékár fendő baranság bengerben vező valamennyi mázslánnak. A ralortás szelles viki, mely a bagma készlező proka alatt szölködik.",
                        Lead = false,
                        UserId = 3
                    }

                }
                ;

            _context.Articles.Add(defaultArticles[0]);
            _context.SaveChanges();
            _context.Articles.Add(defaultArticles[1]);
            _context.SaveChanges();
            for (int i = 2; i < defaultArticles.Length; ++i)
            {               
                _context.Articles.Add(defaultArticles[i]);
            }
            _context.SaveChanges();
        }

        private static void SeedImages(string imageDirectory)
        {
            // Ellenőrizzük, hogy képek könyvtára létezik-e.
            if (Directory.Exists(imageDirectory))
            {
                var images = new List<Picture>();
                var largePath = Path.Combine(imageDirectory, "elso1.png");
                var smallPath = Path.Combine(imageDirectory, "elso1_thumbnail.png");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }
                largePath = Path.Combine(imageDirectory, "elso2.png");
                smallPath = Path.Combine(imageDirectory, "elso2_thumbnail.png");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }
                largePath = Path.Combine(imageDirectory, "elso3.png");
                smallPath = Path.Combine(imageDirectory, "elso3_thumbnail.png");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 1,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }
                largePath = Path.Combine(imageDirectory, "masodik1.png");
                smallPath = Path.Combine(imageDirectory, "masodik1_thumbnail.png");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 2,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }
                largePath = Path.Combine(imageDirectory, "masodik2.png");
                smallPath = Path.Combine(imageDirectory, "masodik2_thumbnail.png");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Picture
                    {
                        ArticleId = 2,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }
                foreach (var image in images)
                {
                    _context.Pictures.Add(image);
                }
                _context.SaveChanges();
            }
        }
    }
}
