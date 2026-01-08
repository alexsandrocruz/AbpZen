import { useEffect, useState } from "react";
import { useRoute, useLocation } from "wouter";
import { Loader2 } from "lucide-react";
import { BlockRenderer } from "@/site-builder/blocks/renderers";
import { BlockContent } from "@shared/schema";

interface SiteData {
  site: {
    id: string;
    name: string;
    primaryColor: string;
    secondaryColor: string;
    fontFamily: string;
    logoUrl?: string;
    faviconUrl?: string;
  };
  page?: {
    id: string;
    title: string;
    content: string;
  };
  pages: {
    id: string;
    title: string;
    slug: string;
  }[];
  preview?: boolean;
}


export default function PublicSitePage() {
  const [, params] = useRoute("/site/:workspaceSlug/:siteSlug/:rest*");
  const [location] = useLocation();
  const [data, setData] = useState<SiteData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchSite() {
      try {
        setLoading(true);
        const res = await fetch(`/api/public${location}`);
        if (!res.ok) {
          const err = await res.json();
          throw new Error(err.message || "Site não encontrado");
        }
        const siteData = await res.json();
        setData(siteData);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    if (location.startsWith("/site/")) {
      fetchSite();
    }
  }, [location]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <Loader2 className="w-8 h-8 animate-spin text-blue-600" />
      </div>
    );
  }

  if (error || !data) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
        <h1 className="text-2xl font-bold text-gray-800 mb-2">Site não encontrado</h1>
        <p className="text-gray-600">{error || "O site solicitado não existe ou não está publicado."}</p>
      </div>
    );
  }

  const { site, page, pages } = data;
  
  let blocks: BlockContent[] = [];
  try {
    blocks = page?.content ? JSON.parse(page.content) : [];
  } catch (e) {
    console.error("Error parsing page content:", e);
  }

  return (
    <div 
      className="min-h-screen"
      style={{ 
        fontFamily: site.fontFamily || 'Inter, system-ui, sans-serif',
        '--primary': site.primaryColor || '#3b82f6',
        '--secondary': site.secondaryColor || '#8b5cf6',
      } as React.CSSProperties}
    >
      {data.preview && (
        <div className="bg-yellow-500 text-black text-center py-2 text-sm font-medium">
          Modo de Visualização - Este site ainda não foi publicado
        </div>
      )}

      <header className="bg-white shadow-sm sticky top-0 z-50">
        <div className="container mx-auto max-w-6xl px-4 py-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            {site.logoUrl && (
              <img src={site.logoUrl} alt={site.name} className="h-10" />
            )}
            <span className="text-xl font-bold">{site.name}</span>
          </div>
          
          <nav className="hidden md:flex items-center gap-6">
            {pages.map((p) => (
              <a 
                key={p.id}
                href={`/site/${params?.workspaceSlug}/${params?.siteSlug}/${p.slug}`}
                className="text-gray-600 hover:text-gray-900 transition-colors"
              >
                {p.title}
              </a>
            ))}
          </nav>
        </div>
      </header>

      <main>
        {blocks.length > 0 ? (
          blocks.map((block) => (
            <BlockRenderer 
              key={block.id} 
              block={block} 
              siteContext={{
                siteId: site.id,
                workspaceSlug: params?.workspaceSlug,
                siteSlug: params?.siteSlug,
              }}
            />
          ))
        ) : (
          <div className="py-20 text-center text-gray-500">
            Esta página ainda não possui conteúdo.
          </div>
        )}
      </main>

      <footer className="bg-gray-900 text-white py-12">
        <div className="container mx-auto max-w-6xl px-4 text-center">
          <p className="text-gray-400">
            &copy; {new Date().getFullYear()} {site.name}. Todos os direitos reservados.
          </p>
        </div>
      </footer>
    </div>
  );
}
