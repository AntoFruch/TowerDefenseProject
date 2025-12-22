# Definition du Projet, Spécificités

But : Créer un jeu type Tower Defense

## Tâches
**Chargement et rendu d’une carte :**
* Chargement et visualisation d’une carte à partir du dossier Maps.
    * 6.1 et 6.4
    * FAIT ( <code>MapManager.cs</code> gère ca tout seul )
    * possibilité de changer les prefabs correspondant au tuiles dans les Styles de map ( dossier Assets/Ressources/MapStyles )

* Création du graphe des chemins.
    * FAIT
    *  _A FAIRE PLUS TARD_ : modifier le poids des aretes avec le recouvrement ( pour l'instant c'est que distance de Manhattan )

* Vérification de la jouabilité de la carte.
    * 6.2
    * FAIT

* Controlleur de caméra permettant de se déplacer sur la carte.
    * 6.4
    * FAIT (<code>ControleCamera.cs</code>)
    * Bonus : Ajouter la possibilité de zoomer/dézoomer sur la caméra
    * Bonus : Ajouter un controle de la caméra à la souris, par drag and drop pour le déplacement et molette pour le zoom

**Développement du jeu :**
* Gestion déroulement du jeu.
    * 7
    * Ca doit alterner entre phase de preparation et phase de defense.( N vagues a survivre puis on peut ameliorer etc)
* Ajout, suppression de bâtiments et de tours.
    * Menu HUD pour choisir
    * Je suis d'avis qu'on clique quelque sur une tuile puis que ca nous propose quoi mettre / combien ca nous coûte.
    * on clique sur un batiment et ca nous propose quoi faire ( ameliorer,  supprimer, modifer ? )
* Spawn et déplacement des monstres.
    * 9.1.1
    * FAIT : Définir les prefabs de monstres à utiliser 
    * Bonus : Avoir des comportements différents en fonction des monstres (plus sur, plus court, aléatoire, ...)
* Gestion des actions des tours et bâtiments.
    * Tours
        * 9.2
        * Tours avec améliorations 
        * caractéristiques variables : Portée, Degats, Cadence
        * Bonus: Le choix des cibles des tours varie en fonction du type de tour (plus proche, plus éloigné, avec le moins de point de point de vie, . . . )
    * Batiments
        * Centrales
            * 9.3.1
        * Installations
            * 9.3.2
            * En gros ya 3 types d'installations qui boostent les defenses sur certains critères

* Visualisation des tirs des tours.
    * 9.2.1
    * On a les prefabs de projectiles
    * à voir une fois les ennemis faits
    * Bonus: Les projectiles et les trajectoires changent en fonction des types de tours

* Gestion de l’energie (centrale et graphe de distribution).
    * 9.4 un peu
    * Graphe d'énérgie -> different du graphe des chemins

* Menus pour pouvoir choisir une carte et lancer une partie.
    * 10.1, 10.2
    * Nouvelle scène
    * UI Document + UXML
    * recupérer les maps disponibles dans le dossier Maps 
        * regarder dans MapManager.cs pour voir comment recuperer un fichier depuis un repertoire externe au projet
    * afficher les noms des map disponibles 
    * cliquer sur le nom de la map -> charge la scene avec la bonne map 
        * jsp comment changer le drag and drop de MapManager avec du code ou meme si c'est comme ca que faut faire mais on demandera au prof / on se documentera
    *  _eventuellement preview de la map mais pas sûr_

* Système de paramêtres sauvegardés dans les PlayerPrefs Unity.
    * Gestion des touches ?
    * taille du HUD ?
    * ptet d'autres trucs faut réfléchir

* Menus de maniere génrérale 
    * 10