# Definition du Projet, Spécificités

But : Créer un jeu type Tower Defense

## Tâches
**Chargement et rendu d’une carte :**
* Chargement et visualisation d’une carte à partir du dossier Maps.
    * FAIT ( <code>MapManager.cs</code> gère ca tout seul)
    * possibilité de changer les prefabs correspondant au tuiles dans les Styles de map ( dossier Assets/Ressources/MapStyles )
* Création du graphe des chemins.
    * pas trop compris ce que ca veut dire
* Vérification de la jouabilité de la carte.
    * faut faire un algo qui montre que un point vert est relié a un point rouge dans tout les cas
* Controlleur de caméra permettant de se déplacer sur la carte.
    * FAIT (<code>ControleCamera.cs</code>)

**Développement du jeu :**
* Gestion déroulement du jeu.
    * _IDEE_ : Par vagues :
        * ca nous propose de nous préparer avec une certaine somme d'argent par default
        * on peut placer ce qu'on veut avec la thune qu'on a
        * Apres on clique sur lancer, ca lance les vagues faut resister aux vagues qui sont de plus en plus grosses avec des monstres de plus en plus coriaces
        * Apres N vagues on peut améliorer / construire, et on relance d'autres vagues 
        * BUT : survivre au max de vagues
    * _IDEE_ : Jeu continu
        * Les vagues arrivent sans preiode de preparation, on peut a tout moment supprimer / ajouter / ameliorer des batiments
* Ajout, suppression de bâtiments et de tours.
    * Menu HUD pour choisir
    * Je suis d'avis qu'on clique quelque sur une tuile puis que ca nous propose quoi mettre / combien ca nous coûte.
    * on clique sur un batiment et ca nous propose quoi faire ( ameliorer,  supprimer, modifer ? )
* Spawn et déplacement des monstres.
    * Définir les prefabs de monstres à utiliser
    * IA des monstres ( focus les batiments, suivre les chemins, Dijkstra pour le focus des batiments ? )
* Gestion des actions des tours et bâtiments.
    * Definir quels bâtiments on fait
    * _MUST_ : 
        * Tours avec améliorations 
        * Centrales ( avec améliorations eventuellement )
    * Penser à d'autres defenses avec d'autres mecanismes (effet de zone ?)
    * des barrieres à placer sur les chemins pour ralentir la progression ?

* Visualisation des tirs des tours.
    * On a les prefabs de projectiles
    * à voir une fois les ennemis et bâtiments faits
* Gestion de l’energie (centrale et graphe de distribution).
    * _IDÉE_ : Centrale qui met de l'energie dans une sphere de X de rayon
    * _PROBLEME_ : est ce que ca respecte l'histoire du graphe ?
* Menus pour pouvoir choisir une carte et lancer une partie.
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