import { useState, useEffect, useRef } from "react";
import { useAuth } from "@/lib/auth-store";
import { api } from "@/lib/api";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Bell, Search, Plus, User, CreditCard, Users, Building2, FolderKanban, FileText, Receipt, ClipboardList, Briefcase, Loader2, MessageSquare, Menu } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Link, useLocation } from "wouter";

interface SearchResult {
  type: string;
  id: string;
  title: string;
  subtitle: string;
  url: string;
}

const typeIcons: Record<string, React.ReactNode> = {
  client: <Building2 className="size-4" />,
  project: <FolderKanban className="size-4" />,
  task: <ClipboardList className="size-4" />,
  proposal: <FileText className="size-4" />,
  invoice: <Receipt className="size-4" />,
  transaction: <Receipt className="size-4" />,
  contract: <Briefcase className="size-4" />,
};

const typeLabels: Record<string, string> = {
  client: 'Cliente',
  project: 'Projeto',
  task: 'Tarefa',
  proposal: 'Proposta',
  invoice: 'Fatura',
  transaction: 'Transação',
  contract: 'Contrato',
};

interface TopbarProps {
  onMenuClick?: () => void;
}

export function Topbar({ onMenuClick }: TopbarProps) {
  const { user, currentWorkspace, logout } = useAuth();
  const [, navigate] = useLocation();
  const [searchQuery, setSearchQuery] = useState("");
  const [results, setResults] = useState<SearchResult[]>([]);
  const [isSearching, setIsSearching] = useState(false);
  const [showResults, setShowResults] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setShowResults(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  useEffect(() => {
    const debounce = setTimeout(async () => {
      if (searchQuery.length >= 2 && currentWorkspace) {
        setIsSearching(true);
        try {
          const data = await api.globalSearch(currentWorkspace.id, searchQuery);
          setResults(data);
          setShowResults(true);
        } catch (error) {
          console.error("Search error:", error);
          setResults([]);
        } finally {
          setIsSearching(false);
        }
      } else {
        setResults([]);
        setShowResults(false);
      }
    }, 300);

    return () => clearTimeout(debounce);
  }, [searchQuery, currentWorkspace]);

  const handleResultClick = (result: SearchResult) => {
    setShowResults(false);
    setSearchQuery("");
    navigate(`/${currentWorkspace?.slug}${result.url}`);
  };

  return (
    <header className="h-16 border-b bg-background/80 backdrop-blur-md sticky top-0 z-20 px-4 lg:px-6 flex items-center justify-between">
      <div className="flex items-center gap-3 flex-1">
        {onMenuClick && (
          <button
            onClick={onMenuClick}
            className="lg:hidden p-2 rounded-md hover:bg-accent"
            data-testid="mobile-menu-button"
          >
            <Menu className="size-5" />
          </button>
        )}
        <div ref={searchRef} className="relative w-full max-w-md hidden md:block">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
          <input 
            ref={inputRef}
            type="text" 
            placeholder="Buscar clientes, projetos, tarefas..." 
            className="w-full h-9 pl-9 pr-4 rounded-full bg-secondary/50 border-none focus:ring-1 focus:ring-primary text-sm transition-all"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onFocus={() => results.length > 0 && setShowResults(true)}
            data-testid="input-global-search"
          />
          {isSearching && (
            <Loader2 className="absolute right-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground animate-spin" />
          )}
          
          {showResults && (
            <div className="absolute top-full left-0 right-0 mt-2 bg-popover border rounded-lg shadow-lg max-h-[400px] overflow-y-auto z-50" data-testid="search-results-dropdown">
              {results.length > 0 ? (
                <div className="py-2">
                  {results.map((result) => (
                    <button
                      key={`${result.type}-${result.id}`}
                      className="w-full px-4 py-2 flex items-center gap-3 hover:bg-accent text-left transition-colors"
                      onClick={() => handleResultClick(result)}
                      data-testid={`search-result-${result.type}-${result.id}`}
                    >
                      <div className="flex-shrink-0 text-muted-foreground">
                        {typeIcons[result.type] || <FileText className="size-4" />}
                      </div>
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate">{result.title}</p>
                        <p className="text-xs text-muted-foreground truncate">{result.subtitle}</p>
                      </div>
                      <span className="text-xs text-muted-foreground bg-muted px-2 py-0.5 rounded">
                        {typeLabels[result.type] || result.type}
                      </span>
                    </button>
                  ))}
                </div>
              ) : (
                <div className="p-4 text-center text-sm text-muted-foreground">
                  Nenhum resultado encontrado para "{searchQuery}"
                </div>
              )}
            </div>
          )}
        </div>
      </div>

      <div className="flex items-center gap-4">
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button size="sm" className="hidden sm:flex gap-2 rounded-full shadow-sm hover:shadow-md transition-all" data-testid="button-novo">
              <Plus className="size-4" />
              <span>Novo</span>
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-48">
            <DropdownMenuLabel>Criar Novo</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <Link href={`/${currentWorkspace?.slug}/clients?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-novo-cliente">
                <Building2 className="size-4 mr-2" />
                Cliente
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/projects?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-novo-projeto">
                <FolderKanban className="size-4 mr-2" />
                Projeto
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/tasks?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-nova-tarefa">
                <ClipboardList className="size-4 mr-2" />
                Tarefa
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/proposals?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-nova-proposta">
                <FileText className="size-4 mr-2" />
                Proposta
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/invoices?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-nova-fatura">
                <Receipt className="size-4 mr-2" />
                Fatura
              </DropdownMenuItem>
            </Link>
            <DropdownMenuSeparator />
            <Link href={`/${currentWorkspace?.slug}/messages?new=true`}>
              <DropdownMenuItem className="cursor-pointer" data-testid="menu-nova-conversa">
                <MessageSquare className="size-4 mr-2" />
                Conversa
              </DropdownMenuItem>
            </Link>
          </DropdownMenuContent>
        </DropdownMenu>

        <button className="relative p-2 rounded-full hover:bg-secondary text-muted-foreground hover:text-foreground transition-colors">
          <Bell className="size-5" />
          <span className="absolute top-1.5 right-1.5 size-2 bg-destructive rounded-full border-2 border-background" />
        </button>

        <div className="h-6 w-px bg-border mx-1" />

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <button className="flex items-center gap-2 hover:bg-secondary/50 p-1.5 pr-3 rounded-full transition-colors outline-none focus:ring-2 focus:ring-primary/20">
              <Avatar className="size-8 border border-border">
                <AvatarImage src={user?.avatar} />
                <AvatarFallback>{user?.name.charAt(0)}</AvatarFallback>
              </Avatar>
              <div className="text-left hidden md:block">
                <p className="text-sm font-medium leading-none">{user?.name}</p>
                <p className="text-xs text-muted-foreground mt-0.5">{currentWorkspace?.name}</p>
              </div>
            </button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-56">
            <DropdownMenuLabel>Minha Conta</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <Link href="/perfil">
              <DropdownMenuItem data-testid="menu-profile">
                <User className="mr-2 h-4 w-4" />
                Perfil
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/cobranca`}>
              <DropdownMenuItem data-testid="menu-billing">
                <CreditCard className="mr-2 h-4 w-4" />
                Cobrança
              </DropdownMenuItem>
            </Link>
            <Link href={`/${currentWorkspace?.slug}/equipe`}>
              <DropdownMenuItem data-testid="menu-team">
                <Users className="mr-2 h-4 w-4" />
                Equipe
              </DropdownMenuItem>
            </Link>
            <DropdownMenuSeparator />
            <DropdownMenuItem 
              className="text-destructive focus:text-destructive"
              onClick={() => logout()}
            >
              Sair
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  );
}
