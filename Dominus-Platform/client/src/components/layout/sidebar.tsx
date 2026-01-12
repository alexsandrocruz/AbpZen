import { Link, useLocation } from "wouter";
import { cn } from "@/lib/utils";
import { 
  LayoutDashboard, 
  Users, 
  Briefcase, 
  CheckSquare, 
  Clock,
  FileText, 
  Receipt, 
  Package,
  DollarSign,
  FileSignature,
  Settings, 
  LogOut,
  Command,
  Filter,
  FileInput,
  Globe,
  CalendarDays,
  UserPlus,
  Layers,
  MessageSquare,
  FolderOpen,
  Zap,
  Bot,
  Crown,
  X,
} from "lucide-react";
import { useAuth } from "@/lib/auth-store";
import { useEffect, useRef } from "react";

interface SidebarProps {
  isOpen?: boolean;
  onClose?: () => void;
}

export function Sidebar({ isOpen = true, onClose }: SidebarProps) {
  const [location] = useLocation();
  const { currentWorkspace, logout, user } = useAuth();
  const previousLocation = useRef(location);
  const wasOpenRef = useRef(false);

  useEffect(() => {
    wasOpenRef.current = isOpen;
  }, [isOpen]);

  useEffect(() => {
    if (previousLocation.current !== location && wasOpenRef.current && onClose) {
      onClose();
    }
    previousLocation.current = location;
  }, [location, onClose]);

  const navItems = [
    { label: "Painel", icon: LayoutDashboard, href: `/${currentWorkspace?.slug}/dashboard` },
    { label: "Clientes", icon: Users, href: `/${currentWorkspace?.slug}/clients` },
    { label: "Projetos", icon: Briefcase, href: `/${currentWorkspace?.slug}/projects` },
    { label: "Tarefas", icon: CheckSquare, href: `/${currentWorkspace?.slug}/tasks` },
    { label: "Tempo", icon: Clock, href: `/${currentWorkspace?.slug}/time` },
    { label: "Produtos", icon: Package, href: `/${currentWorkspace?.slug}/products` },
    { label: "Propostas", icon: FileText, href: `/${currentWorkspace?.slug}/proposals` },
    { label: "Modelos", icon: Layers, href: `/${currentWorkspace?.slug}/proposal-templates` },
    { label: "Contratos", icon: FileSignature, href: `/${currentWorkspace?.slug}/contracts` },
    { label: "Faturas", icon: Receipt, href: `/${currentWorkspace?.slug}/invoices` },
    { label: "Financeiro", icon: DollarSign, href: `/${currentWorkspace?.slug}/financeiro` },
  ];

  const captacaoItems = [
    { label: "Leads", icon: UserPlus, href: `/${currentWorkspace?.slug}/leads` },
    { label: "Funis", icon: Filter, href: `/${currentWorkspace?.slug}/leads/workflows` },
    { label: "Formulários", icon: FileInput, href: `/${currentWorkspace?.slug}/leads/forms` },
    { label: "Landing Pages", icon: Globe, href: `/${currentWorkspace?.slug}/leads/landing-pages` },
    { label: "Agendamentos", icon: CalendarDays, href: `/${currentWorkspace?.slug}/schedulers` },
    { label: "Sites", icon: Layers, href: `/${currentWorkspace?.slug}/sites` },
  ];

  const colaboracaoItems = [
    { label: "Mensagens", icon: MessageSquare, href: `/${currentWorkspace?.slug}/messages` },
    { label: "Arquivos", icon: FolderOpen, href: `/${currentWorkspace?.slug}/files` },
  ];

  const automacaoItems = [
    { label: "Workflows", icon: Zap, href: `/${currentWorkspace?.slug}/workflows` },
    { label: "Super AI", icon: Bot, href: `/${currentWorkspace?.slug}/super-ai` },
  ];

  if (!currentWorkspace) return null;

  const handleNavClick = () => {
    if (onClose) {
      onClose();
    }
  };

  return (
    <>
      {isOpen && onClose && (
        <div 
          className="fixed inset-0 bg-black/50 z-40 lg:hidden"
          onClick={onClose}
          data-testid="sidebar-backdrop"
        />
      )}
      
      <div 
        className={cn(
          "h-screen w-64 border-r bg-sidebar text-sidebar-foreground flex flex-col fixed left-0 top-0 z-50 transition-transform duration-300 ease-in-out",
          "lg:translate-x-0 lg:z-30",
          isOpen ? "translate-x-0" : "-translate-x-full"
        )}
      >
        <div className="h-16 flex items-center justify-between px-6 border-b border-sidebar-border">
          <div className="flex items-center gap-2 font-display font-bold text-xl tracking-tight">
            <div className="size-8 rounded-lg bg-primary text-primary-foreground flex items-center justify-center">
              <Command className="size-5" />
            </div>
            <span>Dominus</span>
          </div>
          {onClose && (
            <button
              onClick={onClose}
              className="lg:hidden p-1 rounded-md hover:bg-sidebar-accent"
              data-testid="sidebar-close-button"
            >
              <X className="size-5" />
            </button>
          )}
        </div>

        <div className="flex-1 py-6 px-3 space-y-1 overflow-y-auto">
          {navItems.map((item) => {
            const isActive = location === item.href || location.startsWith(`${item.href}/`);
            return (
              <Link key={item.href} href={item.href} onClick={handleNavClick}>
                <div
                  className={cn(
                    "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                    isActive
                      ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                      : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                  )}
                >
                  <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                  {item.label}
                  {isActive && (
                    <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                  )}
                </div>
              </Link>
            );
          })}

          <div className="pt-4 mt-4 border-t border-sidebar-border">
            <span className="px-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Captação
            </span>
            <div className="mt-2 space-y-1">
              {captacaoItems.map((item) => {
                const isActive = location === item.href || location.startsWith(`${item.href}/`);
                return (
                  <Link key={item.href} href={item.href} onClick={handleNavClick}>
                    <div
                      className={cn(
                        "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                        isActive
                          ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                          : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                      )}
                    >
                      <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                      {item.label}
                      {isActive && (
                        <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                      )}
                    </div>
                  </Link>
                );
              })}
            </div>
          </div>

          <div className="pt-4 mt-4 border-t border-sidebar-border">
            <span className="px-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Colaboração
            </span>
            <div className="mt-2 space-y-1">
              {colaboracaoItems.map((item) => {
                const isActive = location === item.href || location.startsWith(`${item.href}/`);
                return (
                  <Link key={item.href} href={item.href} onClick={handleNavClick}>
                    <div
                      className={cn(
                        "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                        isActive
                          ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                          : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                      )}
                    >
                      <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                      {item.label}
                      {isActive && (
                        <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                      )}
                    </div>
                  </Link>
                );
              })}
            </div>
          </div>

          <div className="pt-4 mt-4 border-t border-sidebar-border">
            <span className="px-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Automação
            </span>
            <div className="mt-2 space-y-1">
              {automacaoItems.map((item) => {
                const isActive = location === item.href || location.startsWith(`${item.href}/`);
                return (
                  <Link key={item.href} href={item.href} onClick={handleNavClick}>
                    <div
                      className={cn(
                        "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                        isActive
                          ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                          : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                      )}
                    >
                      <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                      {item.label}
                      {isActive && (
                        <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                      )}
                    </div>
                  </Link>
                );
              })}
            </div>
          </div>
        </div>

        <div className="p-3 border-t border-sidebar-border space-y-1">
          {user?.globalRole === 'HOST' && (
            <Link href="/host/dashboard" onClick={handleNavClick}>
              <div
                className={cn(
                  "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors cursor-pointer",
                  location.startsWith("/host")
                    ? "bg-amber-500/20 text-amber-500"
                    : "text-amber-500 hover:bg-amber-500/10"
                )}
                data-testid="nav-host-panel"
              >
                <Crown className="size-5" />
                Painel Host
              </div>
            </Link>
          )}
          <Link href={`/${currentWorkspace.slug}/settings`} onClick={handleNavClick}>
            <div
              className={cn(
                "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors cursor-pointer",
                location.includes("/settings")
                  ? "bg-sidebar-accent text-sidebar-accent-foreground"
                  : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
              )}
            >
              <Settings className="size-5" />
              Configurações
            </div>
          </Link>
          <button
            onClick={() => logout()}
            className="w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium text-muted-foreground hover:bg-destructive/10 hover:text-destructive transition-colors"
          >
            <LogOut className="size-5" />
            Sair
          </button>
        </div>
      </div>
    </>
  );
}
