using Microsoft.AspNetCore.Components;

namespace MudBlazor.Markdown.Core.Pages;

public partial class TableOfContents : ComponentBase
{
    private const string Markdown1 =
        """
        # header 1
        Aenean et ante hendrerit, cursus nunc eget, dignissim risus. Curabitur imperdiet erat mauris, nec feugiat est maximus et. Aliquam consectetur justo est, id dictum ligula hendrerit a. Nam sed condimentum nulla, quis consectetur est. Duis purus neque, lacinia cursus cursus quis, blandit vitae lectus. Nullam condimentum quam sed mi pellentesque bibendum. Pellentesque vestibulum suscipit ligula, cursus consequat ipsum porta varius. Etiam eu metus vitae nunc rutrum sollicitudin. Aenean at volutpat eros, at luctus metus. Suspendisse ullamcorper lacus neque, at volutpat dui facilisis vel. Cras non massa sit amet lacus tincidunt facilisis. Nunc varius elit et erat lacinia lacinia.
        
        # header 2
        Praesent blandit ante ac diam malesuada, quis condimentum mi cursus. Ut gravida tincidunt nulla, eu tristique augue iaculis vel. Aenean porttitor sapien vel magna tincidunt sollicitudin. Ut sit amet mi at quam ornare egestas. Nulla fringilla metus eros, non mollis sapien luctus congue. Donec nec dolor id augue pellentesque porta. Nulla facilisi.
        
        # header 3
        Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nulla quis vestibulum odio. Morbi pulvinar consequat lacus a dapibus. Aliquam erat volutpat. Nulla non lectus risus. Pellentesque tincidunt at libero a feugiat. Donec sem odio, pellentesque sed bibendum ut, ultricies vel tortor.
        
        ## header 3-1
        Nunc tincidunt quam neque, vel laoreet libero molestie a. Suspendisse dictum, velit non vulputate vulputate, ipsum ante aliquam massa, et scelerisque leo metus ac nulla. Aliquam erat volutpat. Nullam sed eros augue. Vestibulum interdum porttitor augue. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Pellentesque tempor tortor purus, at molestie nunc tempus at. Sed volutpat mauris non ligula feugiat, eu dignissim diam volutpat. Etiam varius, nulla quis pellentesque suscipit, sapien lacus facilisis augue, quis pellentesque justo arcu vehicula ante. Sed urna urna, tincidunt quis mattis quis, sagittis sit amet tortor. In id eros sollicitudin, semper odio vitae, sagittis diam.
        
        ## header 3-2
        Sed sed aliquam risus, a tincidunt magna. Etiam nec augue at massa porttitor mattis. Donec in suscipit mauris. Vivamus nisi sapien, accumsan eget purus vitae, porta commodo urna. Donec auctor volutpat ornare. Pellentesque vitae ultrices lacus. Praesent sit amet urna ut tortor mattis tempor quis at diam. Vivamus eu porta arcu, vitae vehicula sem.
        
        ### header 3-2-1
        Suspendisse potenti. Sed ac metus fermentum, feugiat felis lobortis, dapibus justo. Aenean ornare ut lorem quis bibendum. Aliquam accumsan elit ut metus interdum hendrerit. Fusce vel porta libero. Mauris non hendrerit tortor. Aliquam quis nisi lacinia, blandit erat in, facilisis tellus. Nam aliquet diam tortor, id aliquet metus tempus ac. Sed sem lectus, feugiat sit amet imperdiet sed, scelerisque a arcu. Sed gravida orci risus, ullamcorper ultricies urna viverra eu. Donec imperdiet a ipsum id fermentum. Nulla facilisi. Sed interdum enim tortor, quis dictum est finibus sed. Nunc et augue porta, luctus nisl vulputate, finibus augue.
        
        ### header 3-2-2
        Ut pellentesque ex et facilisis vulputate. Donec eu orci nec odio ullamcorper auctor ut sed nulla. Integer accumsan magna metus, nec posuere sapien volutpat venenatis. Duis viverra aliquam fermentum. Maecenas suscipit ex pharetra arcu dignissim tristique. Nunc enim dolor, facilisis at purus sit amet, fermentum pretium enim. Maecenas consequat, dolor ut maximus facilisis, diam risus rhoncus dolor, accumsan vehicula augue ante nec felis. Phasellus congue dolor non risus rhoncus, eu suscipit massa tincidunt. Vivamus fermentum, tortor at molestie tempus, justo arcu iaculis urna, sagittis efficitur nulla erat vitae urna. Ut sollicitudin elementum quam eget blandit.
        
        # header 4
        Cras posuere nunc eget eros accumsan molestie. Suspendisse a lacus eu mi placerat vulputate. Nulla pharetra magna vel eleifend finibus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Phasellus gravida justo egestas lectus porttitor gravida. Ut libero velit, tempus lacinia efficitur interdum, vestibulum eu quam. Proin molestie fringilla nunc, eget sollicitudin enim rhoncus nec. Curabitur at eros congue sapien maximus efficitur.
        """;

    private const string Markdown2 =
        """
        # header2 1
        Donec hendrerit, velit sit amet consequat eleifend, tortor est iaculis lectus, vel elementum diam nisl non quam. Etiam ut justo vitae nibh pretium laoreet. Praesent lorem mauris, viverra ac mattis ut, viverra sit amet sem. Phasellus dictum viverra dui, sit amet rutrum risus laoreet a. Morbi in ante vitae augue vehicula auctor non sit amet ligula. Aliquam eleifend finibus dapibus. Fusce placerat nibh eu odio vehicula porttitor. Aliquam dignissim rutrum leo eget tempus.
        
        # header2 2
        Sed interdum consectetur dolor, a vestibulum augue ultricies quis. Ut id urna nec nunc condimentum maximus ut non erat. Ut sagittis elit a nisl iaculis, tempus dignissim mi volutpat. Vestibulum vitae sem vel mauris semper convallis. In porta fermentum lacinia. Duis pellentesque laoreet urna, et varius nisl accumsan vel. Maecenas tristique efficitur felis nec volutpat. Praesent enim neque, mattis lacinia gravida semper, auctor ac purus.
        
        # header2 3
        Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Proin vitae cursus erat, id pretium diam. Proin fermentum tincidunt pellentesque. Maecenas sed mollis elit. In commodo nulla ac gravida rhoncus. Curabitur auctor justo eu imperdiet vestibulum. Integer in erat leo. Etiam lorem odio, eleifend ut arcu eu, venenatis faucibus nisi. Vivamus a massa sollicitudin, semper neque at, pharetra arcu. Nulla nibh justo, condimentum eget placerat nec, mollis a ante. Aenean purus odio, ornare nec auctor vel, efficitur pulvinar nunc. Donec blandit augue sit amet orci pharetra vestibulum.
        
        ## header2 3-1
        Nam vulputate ipsum vitae dolor facilisis facilisis. Maecenas sed auctor nulla, eu blandit mi. Proin aliquam, sapien nec gravida tincidunt, diam tortor interdum libero, nec rutrum risus orci eu neque. Morbi congue risus lorem, et eleifend tellus feugiat sed. Nullam a turpis mauris. Fusce purus mi, tincidunt a lacus id, imperdiet volutpat ante. Nunc lobortis nisi lorem. Cras fermentum luctus ligula, et finibus mauris tristique a. Morbi vestibulum ipsum non eleifend consectetur. Quisque non ultrices diam, quis vehicula ligula. Duis dignissim blandit commodo. Integer eu tincidunt metus, a pharetra nisl. Fusce faucibus nunc arcu, in maximus velit luctus quis. Fusce sodales ipsum augue, eu pharetra purus hendrerit eu.
        
        ## header2 3-2
        Mauris blandit imperdiet nibh, viverra dapibus arcu. Nunc tempor leo a convallis pretium. Donec luctus nunc a viverra gravida. In lacinia elementum porttitor. Nam eu aliquam lorem. Quisque scelerisque risus in justo semper, ut consectetur mi mattis. Etiam eget lorem felis. Sed tortor justo, vulputate eu convallis vitae, tincidunt a leo. Etiam accumsan rhoncus pretium. Duis sed justo at augue molestie consectetur. Sed pharetra malesuada lacinia.
        
        ### header2 3-2-1
        Vestibulum sollicitudin urna vel porta tincidunt. Etiam eu commodo tortor. Praesent elementum nulla mauris, quis efficitur felis scelerisque et. Nunc facilisis, metus in gravida dictum, diam mi ultricies magna, id blandit diam velit ac risus. Phasellus vel tincidunt erat. Nullam vitae sagittis justo. Pellentesque sit amet augue finibus justo hendrerit ultricies. Donec tincidunt faucibus turpis. Vestibulum vestibulum molestie velit, at feugiat ex fermentum ut.
        
        ### header2 3-2-2
        Aliquam iaculis accumsan ligula vel mattis. Etiam id ipsum id metus ultrices tincidunt. Nam quis risus pharetra, ullamcorper augue non, vestibulum tellus. Ut sodales dignissim est, sed aliquam elit. In faucibus pulvinar sapien, sit amet ultrices ante maximus et. Praesent faucibus ex ligula, vitae venenatis urna venenatis eu. Suspendisse quis sapien eleifend, aliquam nibh eget, tempus tellus. Praesent nisl magna, consequat id mauris ut, suscipit dapibus urna. Aenean erat arcu, malesuada nec metus vel, blandit finibus tortor. Curabitur arcu libero, cursus id posuere gravida, lacinia nec metus.
        
        # header2 4
        Curabitur nunc urna, pharetra et sollicitudin a, semper eu risus. Vivamus quam nisi, venenatis sed lectus eu, gravida aliquet odio. Aenean eros nisi, ornare vitae lobortis quis, fermentum sed quam. Aliquam erat volutpat. Phasellus at mauris eu nibh rhoncus volutpat vitae vitae lorem. Praesent eget scelerisque ex, in tincidunt metus. Vestibulum commodo sollicitudin fermentum. Suspendisse ultricies vestibulum pulvinar. Duis et molestie odio. Donec ipsum elit, tincidunt a libero et, venenatis blandit nibh. Nulla sit amet lorem enim. Suspendisse imperdiet neque sed massa tempor, eget sodales mauris volutpat.
        """;
}
