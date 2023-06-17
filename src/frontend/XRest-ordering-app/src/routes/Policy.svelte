<script>
    import LayoutSimple from './_LayoutSimple.svelte'
    import { _ } from '../lib/services/localization/i18n'
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div
            class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo"
                />
            </div>
        </div>
        <div class="mx-auto card my-5 col-md-11 col-xxl-7">
            <div class="card-header bg-white">
                <ul class="nav card-header-pills justify-content-center fs-4 search-address">
                    <li class="nav-item text-uppercase">{$_("policy.title")}</li>
                </ul>
            </div>
            <div class="card-body">
                <h2>Wstęp</h2>
                <ol>
                    <li>Roxana Sp. z o.o., właściciel marki Kebab King, z siedzibą przy Al. Jerozolimskich 42 w Warszawie 00-024, jest Administratorem danych osobowych (Administrator) gromadzonych i przetwarzanych w ramach korzystania z serwisu zamowienia.kebabking.biz. </li>
                    <li>Administrator przetwarza gromadzone dane osobowe z zachowaniem zasad bezpieczeństwa wynikających z przepisów ustawy z dnia 29 sierpnia 1997 r. o ochronie danych osobowych (tekst jednolity: Dz. U. 2002 r. Nr 101 poz. 926, z późn. zm.; dalej o.d.o.) oraz ustawy z dnia 18 lipca 2002 r. o świadczeniu usług drogą elektroniczną (Dz. U. z 2002 r. Nr 144, poz. 1204, z późń. zm; dalej u.ś.u.d.e.).</li>
                    <li>Administrator jest świadom zagrożeń wynikających przetwarzania danych osobowych w sieci Internet, dlatego też oświadcza, iż dokłada starań, aby zapewnić odpowiedni poziom ochrony prywatności i bezpieczeństwa Użytkowników. Stosowane narzędzia zostały dobrane tak, by zapewnić należytą ochronę przetwarzania danych osobowych zgodnie z wymogami prawa.</li>
                </ol>
                <h2>Jakie dane są przetwarzane</h2>
                Przy rejestracji Użytkownik zostaje poproszony o podanie następujących danych, które są niezbędne do założenia Konta Użytkownika: 
                <ul>
                    <li>adres e-mail</li>
                    <li>imię i nazwisko</li>
                    <li>hasło, które Użytkownik wybiera dowolnie</li>
                    To hasło jest przechowywane w postaci zaszyfrowanej i jest nie widoczne w postaci zwykłego tekstu. 
                </ul>
            
                <h2>Informacja o plikach cookies</h2>
                <ol>
                    <li>Serwis korzysta z plików cookies.</li>
                    <li>Pliki cookies (tzw. „ciasteczka”) stanowią dane informatyczne, w szczególności pliki tekstowe, które przechowywane są w urządzeniu końcowym Użytkownika Serwisu i przeznaczone są do korzystania ze stron internetowych Serwisu. Cookies zazwyczaj zawierają nazwę strony internetowej, z której pochodzą, czas przechowywania ich na urządzeniu końcowym oraz unikalny numer.</li>
                    <li>Podmiotem zamieszczającym na urządzeniu końcowym Użytkownika Serwisu pliki cookies oraz uzyskującym do nich dostęp jest operator Serwisu.</li>
                    <li>Pliki cookies wykorzystywane są w następujących celach:</li>
                    <ul>
                        <li>tworzenia statystyk, które pomagają zrozumieć, w jaki sposób Użytkownicy Serwisu korzystają ze stron internetowych, co umożliwia ulepszanie ich struktury i zawartości;</li>
                        <li>utrzymanie sesji Użytkownika Serwisu (po zalogowaniu), dzięki której Użytkownik nie musi na każdej podstronie Serwisu ponownie wpisywać loginu i hasła;</li>
                        <li>określania profilu użytkownika w celu wyświetlania mu dopasowanych materiałów w sieciach reklamowych, w szczególności sieci Google.</li>
                    </ul>
                    <li>W ramach Serwisu stosowane są dwa zasadnicze rodzaje plików cookies: „sesyjne” (session cookies) oraz „stałe” (persistent cookies). Cookies „sesyjne” są plikami tymczasowymi, które przechowywane są w urządzeniu końcowym Użytkownika do czasu wylogowania, opuszczenia strony internetowej lub wyłączenia oprogramowania (przeglądarki internetowej). „Stałe” pliki cookies przechowywane są w urządzeniu końcowym Użytkownika przez czas określony w parametrach plików cookies lub do czasu ich usunięcia przez Użytkownika.</li>
                    <li>Oprogramowanie do przeglądania stron internetowych (przeglądarka internetowa) zazwyczaj domyślnie dopuszcza przechowywanie plików cookies w urządzeniu końcowym Użytkownika. Użytkownicy Serwisu mogą dokonać zmiany ustawień w tym zakresie. Przeglądarka internetowa umożliwia usunięcie plików cookies. Możliwe jest także automatyczne blokowanie plików cookies Szczegółowe informacje na ten temat zawiera pomoc lub dokumentacja przeglądarki internetowej.</li>
                    <li>Ograniczenia stosowania plików cookies mogą wpłynąć na niektóre funkcjonalności dostępne na stronach internetowych Serwisu.</li>
                    <li>Pliki cookies zamieszczane w urządzeniu końcowym Użytkownika Serwisu i wykorzystywane mogą być również przez współpracujących z operatorem Serwisu reklamodawców oraz partnerów.</li>
                    <li>Zalecamy przeczytanie polityki ochrony prywatności tych firm, aby poznać zasady korzystania z plików cookie wykorzystywane w statystykach: Polityka ochrony prywatności Google Analytics</li>
                </ol>
                <h2>Zarządzanie plikami cookies – jak w praktyce wyrażać i cofać zgodę?</h2>
                <ol>
                    <li>Jeśli użytkownik nie chce otrzymywać plików cookies, może zmienić ustawienia przeglądarki. Zastrzegamy, że wyłączenie obsługi plików cookies niezbędnych dla procesów uwierzytelniania, bezpieczeństwa, utrzymania preferencji użytkownika może utrudnić, a w skrajnych przypadkach może uniemożliwić korzystanie ze stron www</li>
                    <li>W celu zarządzania ustawieniami cookies sprawdź ustawienia swojej przeglądarki.</li>
            
                </ol>
            
                <h2>Wyłączenie odpowiedzialności</h2>
                Polityka prywatności nie ma zastosowania do przedsiębiorców oraz osób, których dane są ogólnie dostępne na stronach serwisu. <br />
                Operator nie ponosi również odpowiedzialności za dane udostępnione przez Użytkowników.<br />
                Strony Serwisu mogą zawierać linki do innych stron, które nie są powiązane z Serwisem. Po kliknięciu na link do innych stron (linki zewnętrzne) nie mamy wpływu na to, jakie dane, w jakim celu i w jaki sposób są przetwarzanie i przekazywane do tej strony. Administrator danych osobowych nie ponosi odpowiedzialności za przetwarzanie danych osobowych przez osoby trzecie.
            </div>
        </div>
    </div>
</LayoutSimple>
