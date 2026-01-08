import { useState, useRef } from "react";
import { useParams, useLocation } from "wouter";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth } from "@/lib/auth-store";
import { api } from "@/lib/api";
import { useUpload } from "@/hooks/use-upload";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  ArrowLeft,
  Plus,
  MoreHorizontal,
  Pencil,
  Trash2,
  Eye,
  FileText,
  Tag,
  Calendar,
  User,
  Upload,
  Loader2,
  ImageIcon,
  X,
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import type { BlogPost, BlogCategory, SiteProject } from "@shared/schema";

export default function BlogPostsPage() {
  const { siteId } = useParams<{ siteId: string }>();
  const [, navigate] = useLocation();
  const { currentWorkspace } = useAuth();
  const queryClient = useQueryClient();

  const [activeTab, setActiveTab] = useState("posts");
  const [postDialogOpen, setPostDialogOpen] = useState(false);
  const [categoryDialogOpen, setCategoryDialogOpen] = useState(false);
  const [editingPost, setEditingPost] = useState<BlogPost | null>(null);
  const [editingCategory, setEditingCategory] = useState<BlogCategory | null>(null);

  const [postForm, setPostForm] = useState({
    title: "",
    slug: "",
    excerpt: "",
    content: "",
    coverImage: "",
    categoryId: "",
    status: "DRAFT",
    seoTitle: "",
    seoDescription: "",
  });

  const [categoryForm, setCategoryForm] = useState({
    name: "",
    slug: "",
    description: "",
  });

  const fileInputRef = useRef<HTMLInputElement>(null);
  const { uploadFile, isUploading } = useUpload({
    onSuccess: (response) => {
      setPostForm({ ...postForm, coverImage: response.objectPath });
    },
  });

  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      await uploadFile(file);
    }
  };

  if (!currentWorkspace) {
    return (
      <div className="flex items-center justify-center h-full">
        <p className="text-muted-foreground">Carregando...</p>
      </div>
    );
  }

  const { data: site } = useQuery<SiteProject>({
    queryKey: ["site", siteId],
    queryFn: () => api.getSiteProject(currentWorkspace!.id, siteId!),
    enabled: !!siteId && !!currentWorkspace,
  });

  const { data: posts = [], isLoading: postsLoading } = useQuery<BlogPost[]>({
    queryKey: ["blog-posts", siteId],
    queryFn: () => api.getBlogPosts(siteId!),
    enabled: !!siteId,
  });

  const { data: categories = [], isLoading: categoriesLoading } = useQuery<BlogCategory[]>({
    queryKey: ["blog-categories", siteId],
    queryFn: () => api.getBlogCategories(siteId!),
    enabled: !!siteId,
  });

  const createPostMutation = useMutation({
    mutationFn: (data: any) => api.createBlogPost(siteId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-posts", siteId] });
      setPostDialogOpen(false);
      resetPostForm();
    },
  });

  const updatePostMutation = useMutation({
    mutationFn: ({ postId, data }: { postId: string; data: any }) =>
      api.updateBlogPost(siteId!, postId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-posts", siteId] });
      setPostDialogOpen(false);
      setEditingPost(null);
      resetPostForm();
    },
  });

  const deletePostMutation = useMutation({
    mutationFn: (postId: string) => api.deleteBlogPost(siteId!, postId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-posts", siteId] });
    },
  });

  const createCategoryMutation = useMutation({
    mutationFn: (data: any) => api.createBlogCategory(siteId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-categories", siteId] });
      setCategoryDialogOpen(false);
      resetCategoryForm();
    },
  });

  const updateCategoryMutation = useMutation({
    mutationFn: ({ categoryId, data }: { categoryId: string; data: any }) =>
      api.updateBlogCategory(siteId!, categoryId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-categories", siteId] });
      setCategoryDialogOpen(false);
      setEditingCategory(null);
      resetCategoryForm();
    },
  });

  const deleteCategoryMutation = useMutation({
    mutationFn: (categoryId: string) => api.deleteBlogCategory(siteId!, categoryId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blog-categories", siteId] });
    },
  });

  const resetPostForm = () => {
    setPostForm({
      title: "",
      slug: "",
      excerpt: "",
      content: "",
      coverImage: "",
      categoryId: "",
      status: "DRAFT",
      seoTitle: "",
      seoDescription: "",
    });
  };

  const resetCategoryForm = () => {
    setCategoryForm({
      name: "",
      slug: "",
      description: "",
    });
  };

  const handleOpenPostDialog = (post?: BlogPost) => {
    if (post) {
      setEditingPost(post);
      setPostForm({
        title: post.title,
        slug: post.slug,
        excerpt: post.excerpt || "",
        content: post.content || "",
        coverImage: post.coverImage || "",
        categoryId: post.categoryId || "",
        status: post.status,
        seoTitle: post.seoTitle || "",
        seoDescription: post.seoDescription || "",
      });
    } else {
      setEditingPost(null);
      resetPostForm();
    }
    setPostDialogOpen(true);
  };

  const handleOpenCategoryDialog = (category?: BlogCategory) => {
    if (category) {
      setEditingCategory(category);
      setCategoryForm({
        name: category.name,
        slug: category.slug,
        description: category.description || "",
      });
    } else {
      setEditingCategory(null);
      resetCategoryForm();
    }
    setCategoryDialogOpen(true);
  };

  const handleSavePost = () => {
    const slug = postForm.slug || postForm.title.toLowerCase().replace(/\s+/g, "-").replace(/[^a-z0-9-]/g, "");
    const data = { ...postForm, slug };

    if (editingPost) {
      updatePostMutation.mutate({ postId: editingPost.id, data });
    } else {
      createPostMutation.mutate(data);
    }
  };

  const handleSaveCategory = () => {
    const slug = categoryForm.slug || categoryForm.name.toLowerCase().replace(/\s+/g, "-").replace(/[^a-z0-9-]/g, "");
    const data = { ...categoryForm, slug };

    if (editingCategory) {
      updateCategoryMutation.mutate({ categoryId: editingCategory.id, data });
    } else {
      createCategoryMutation.mutate(data);
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "PUBLISHED":
        return <Badge className="bg-green-500">Publicado</Badge>;
      case "DRAFT":
        return <Badge variant="secondary">Rascunho</Badge>;
      case "ARCHIVED":
        return <Badge variant="outline">Arquivado</Badge>;
      default:
        return <Badge>{status}</Badge>;
    }
  };

  const getCategoryName = (categoryId: string | null) => {
    if (!categoryId) return "-";
    const category = categories.find((c) => c.id === categoryId);
    return category?.name || "-";
  };

  return (
    <div className="flex flex-col h-full">
      <div className="border-b bg-background">
        <div className="container flex items-center gap-4 py-4">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => navigate(`/${currentWorkspace.slug}/sites/${siteId}/builder`)}
            data-testid="button-back"
          >
            <ArrowLeft className="h-4 w-4" />
          </Button>
          <div>
            <h1 className="text-xl font-semibold">
              Blog - {site?.name || "Carregando..."}
            </h1>
            <p className="text-sm text-muted-foreground">
              Gerencie os posts e categorias do blog
            </p>
          </div>
        </div>
      </div>

      <div className="flex-1 container py-6 overflow-auto">
        <Tabs value={activeTab} onValueChange={setActiveTab}>
          <div className="flex justify-between items-center mb-4">
            <TabsList>
              <TabsTrigger value="posts" data-testid="tab-posts">
                <FileText className="h-4 w-4 mr-2" />
                Posts
              </TabsTrigger>
              <TabsTrigger value="categories" data-testid="tab-categories">
                <Tag className="h-4 w-4 mr-2" />
                Categorias
              </TabsTrigger>
            </TabsList>

            {activeTab === "posts" ? (
              <Button onClick={() => handleOpenPostDialog()} data-testid="button-new-post">
                <Plus className="h-4 w-4 mr-2" />
                Novo Post
              </Button>
            ) : (
              <Button onClick={() => handleOpenCategoryDialog()} data-testid="button-new-category">
                <Plus className="h-4 w-4 mr-2" />
                Nova Categoria
              </Button>
            )}
          </div>

          <TabsContent value="posts">
            <Card>
              <CardContent className="p-0">
                {postsLoading ? (
                  <div className="flex items-center justify-center py-12">
                    <p className="text-muted-foreground">Carregando posts...</p>
                  </div>
                ) : posts.length === 0 ? (
                  <div className="flex flex-col items-center justify-center py-12 gap-4">
                    <FileText className="h-12 w-12 text-muted-foreground" />
                    <p className="text-muted-foreground">Nenhum post encontrado</p>
                    <Button onClick={() => handleOpenPostDialog()} data-testid="button-create-first-post">
                      <Plus className="h-4 w-4 mr-2" />
                      Criar primeiro post
                    </Button>
                  </div>
                ) : (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Título</TableHead>
                        <TableHead>Categoria</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead>Data</TableHead>
                        <TableHead>Visualizações</TableHead>
                        <TableHead className="w-[50px]"></TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {posts.map((post) => (
                        <TableRow key={post.id} data-testid={`row-post-${post.id}`}>
                          <TableCell>
                            <div className="flex items-center gap-3">
                              {post.coverImage ? (
                                <img
                                  src={post.coverImage}
                                  alt={post.title}
                                  className="w-12 h-12 object-cover rounded"
                                />
                              ) : (
                                <div className="w-12 h-12 bg-muted rounded flex items-center justify-center">
                                  <FileText className="h-5 w-5 text-muted-foreground" />
                                </div>
                              )}
                              <div>
                                <p className="font-medium">{post.title}</p>
                                <p className="text-sm text-muted-foreground">/{post.slug}</p>
                              </div>
                            </div>
                          </TableCell>
                          <TableCell>{getCategoryName(post.categoryId)}</TableCell>
                          <TableCell>{getStatusBadge(post.status)}</TableCell>
                          <TableCell>
                            {post.publishedAt
                              ? format(new Date(post.publishedAt), "dd/MM/yyyy", { locale: ptBR })
                              : format(new Date(post.createdAt), "dd/MM/yyyy", { locale: ptBR })}
                          </TableCell>
                          <TableCell>{post.viewCount || 0}</TableCell>
                          <TableCell>
                            <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                <Button variant="ghost" size="icon" data-testid={`button-menu-post-${post.id}`}>
                                  <MoreHorizontal className="h-4 w-4" />
                                </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="end">
                                <DropdownMenuItem onClick={() => handleOpenPostDialog(post)}>
                                  <Pencil className="h-4 w-4 mr-2" />
                                  Editar
                                </DropdownMenuItem>
                                <DropdownMenuItem
                                  className="text-destructive"
                                  onClick={() => {
                                    if (confirm("Tem certeza que deseja excluir este post?")) {
                                      deletePostMutation.mutate(post.id);
                                    }
                                  }}
                                >
                                  <Trash2 className="h-4 w-4 mr-2" />
                                  Excluir
                                </DropdownMenuItem>
                              </DropdownMenuContent>
                            </DropdownMenu>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                )}
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="categories">
            <Card>
              <CardContent className="p-0">
                {categoriesLoading ? (
                  <div className="flex items-center justify-center py-12">
                    <p className="text-muted-foreground">Carregando categorias...</p>
                  </div>
                ) : categories.length === 0 ? (
                  <div className="flex flex-col items-center justify-center py-12 gap-4">
                    <Tag className="h-12 w-12 text-muted-foreground" />
                    <p className="text-muted-foreground">Nenhuma categoria encontrada</p>
                    <Button onClick={() => handleOpenCategoryDialog()} data-testid="button-create-first-category">
                      <Plus className="h-4 w-4 mr-2" />
                      Criar primeira categoria
                    </Button>
                  </div>
                ) : (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Nome</TableHead>
                        <TableHead>Slug</TableHead>
                        <TableHead>Descrição</TableHead>
                        <TableHead className="w-[50px]"></TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {categories.map((category) => (
                        <TableRow key={category.id} data-testid={`row-category-${category.id}`}>
                          <TableCell className="font-medium">{category.name}</TableCell>
                          <TableCell>/{category.slug}</TableCell>
                          <TableCell>{category.description || "-"}</TableCell>
                          <TableCell>
                            <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                <Button variant="ghost" size="icon" data-testid={`button-menu-category-${category.id}`}>
                                  <MoreHorizontal className="h-4 w-4" />
                                </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="end">
                                <DropdownMenuItem onClick={() => handleOpenCategoryDialog(category)}>
                                  <Pencil className="h-4 w-4 mr-2" />
                                  Editar
                                </DropdownMenuItem>
                                <DropdownMenuItem
                                  className="text-destructive"
                                  onClick={() => {
                                    if (confirm("Tem certeza que deseja excluir esta categoria?")) {
                                      deleteCategoryMutation.mutate(category.id);
                                    }
                                  }}
                                >
                                  <Trash2 className="h-4 w-4 mr-2" />
                                  Excluir
                                </DropdownMenuItem>
                              </DropdownMenuContent>
                            </DropdownMenu>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                )}
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </div>

      <Dialog open={postDialogOpen} onOpenChange={setPostDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {editingPost ? "Editar Post" : "Novo Post"}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="post-title">Título *</Label>
                <Input
                  id="post-title"
                  value={postForm.title}
                  onChange={(e) => setPostForm({ ...postForm, title: e.target.value })}
                  placeholder="Título do post"
                  data-testid="input-post-title"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="post-slug">Slug</Label>
                <Input
                  id="post-slug"
                  value={postForm.slug}
                  onChange={(e) => setPostForm({ ...postForm, slug: e.target.value })}
                  placeholder="url-do-post"
                  data-testid="input-post-slug"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="post-category">Categoria</Label>
                <Select
                  value={postForm.categoryId}
                  onValueChange={(value) => setPostForm({ ...postForm, categoryId: value })}
                >
                  <SelectTrigger data-testid="select-post-category">
                    <SelectValue placeholder="Selecione uma categoria" />
                  </SelectTrigger>
                  <SelectContent>
                    {categories.map((cat) => (
                      <SelectItem key={cat.id} value={cat.id}>
                        {cat.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="post-status">Status</Label>
                <Select
                  value={postForm.status}
                  onValueChange={(value) => setPostForm({ ...postForm, status: value })}
                >
                  <SelectTrigger data-testid="select-post-status">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="DRAFT">Rascunho</SelectItem>
                    <SelectItem value="PUBLISHED">Publicado</SelectItem>
                    <SelectItem value="ARCHIVED">Arquivado</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="space-y-2">
              <Label>Imagem de Destaque</Label>
              <div className="space-y-3">
                {postForm.coverImage ? (
                  <div className="relative inline-block">
                    <img
                      src={postForm.coverImage}
                      alt="Imagem de destaque"
                      className="w-full max-w-xs h-40 object-cover rounded-lg border"
                    />
                    <Button
                      type="button"
                      variant="destructive"
                      size="icon"
                      className="absolute top-2 right-2 h-6 w-6"
                      onClick={() => setPostForm({ ...postForm, coverImage: "" })}
                    >
                      <X className="h-4 w-4" />
                    </Button>
                  </div>
                ) : (
                  <div className="flex items-center justify-center w-full max-w-xs h-40 bg-muted rounded-lg border-2 border-dashed">
                    <div className="text-center text-muted-foreground">
                      <ImageIcon className="h-10 w-10 mx-auto mb-2" />
                      <p className="text-sm">Nenhuma imagem selecionada</p>
                    </div>
                  </div>
                )}
                <div className="flex items-center gap-2">
                  <input
                    ref={fileInputRef}
                    type="file"
                    accept="image/*"
                    onChange={handleImageUpload}
                    className="hidden"
                    data-testid="input-post-image-file"
                  />
                  <Button
                    type="button"
                    variant="outline"
                    size="sm"
                    onClick={() => fileInputRef.current?.click()}
                    disabled={isUploading}
                    data-testid="button-upload-image"
                  >
                    {isUploading ? (
                      <>
                        <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                        Enviando...
                      </>
                    ) : (
                      <>
                        <Upload className="h-4 w-4 mr-2" />
                        Fazer Upload
                      </>
                    )}
                  </Button>
                  <span className="text-xs text-muted-foreground">ou</span>
                  <Input
                    id="post-image"
                    value={postForm.coverImage}
                    onChange={(e) => setPostForm({ ...postForm, coverImage: e.target.value })}
                    placeholder="Cole uma URL de imagem..."
                    className="flex-1"
                    data-testid="input-post-image"
                  />
                </div>
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="post-excerpt">Resumo</Label>
              <Textarea
                id="post-excerpt"
                value={postForm.excerpt}
                onChange={(e) => setPostForm({ ...postForm, excerpt: e.target.value })}
                placeholder="Breve descrição do post..."
                rows={2}
                data-testid="input-post-excerpt"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="post-content">Conteúdo</Label>
              <Textarea
                id="post-content"
                value={postForm.content}
                onChange={(e) => setPostForm({ ...postForm, content: e.target.value })}
                placeholder="Escreva o conteúdo do post..."
                rows={10}
                data-testid="input-post-content"
              />
            </div>

            <div className="border-t pt-4">
              <h4 className="font-medium mb-3">SEO</h4>
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="post-seo-title">Título SEO</Label>
                  <Input
                    id="post-seo-title"
                    value={postForm.seoTitle}
                    onChange={(e) => setPostForm({ ...postForm, seoTitle: e.target.value })}
                    placeholder="Título para mecanismos de busca"
                    data-testid="input-post-seo-title"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="post-seo-description">Descrição SEO</Label>
                  <Textarea
                    id="post-seo-description"
                    value={postForm.seoDescription}
                    onChange={(e) => setPostForm({ ...postForm, seoDescription: e.target.value })}
                    placeholder="Descrição para mecanismos de busca"
                    rows={2}
                    data-testid="input-post-seo-description"
                  />
                </div>
              </div>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setPostDialogOpen(false)}>
              Cancelar
            </Button>
            <Button
              onClick={handleSavePost}
              disabled={!postForm.title || createPostMutation.isPending || updatePostMutation.isPending}
              data-testid="button-save-post"
            >
              {editingPost ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      <Dialog open={categoryDialogOpen} onOpenChange={setCategoryDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>
              {editingCategory ? "Editar Categoria" : "Nova Categoria"}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="category-name">Nome *</Label>
              <Input
                id="category-name"
                value={categoryForm.name}
                onChange={(e) => setCategoryForm({ ...categoryForm, name: e.target.value })}
                placeholder="Nome da categoria"
                data-testid="input-category-name"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="category-slug">Slug</Label>
              <Input
                id="category-slug"
                value={categoryForm.slug}
                onChange={(e) => setCategoryForm({ ...categoryForm, slug: e.target.value })}
                placeholder="url-da-categoria"
                data-testid="input-category-slug"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="category-description">Descrição</Label>
              <Textarea
                id="category-description"
                value={categoryForm.description}
                onChange={(e) => setCategoryForm({ ...categoryForm, description: e.target.value })}
                placeholder="Descrição da categoria..."
                rows={3}
                data-testid="input-category-description"
              />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setCategoryDialogOpen(false)}>
              Cancelar
            </Button>
            <Button
              onClick={handleSaveCategory}
              disabled={!categoryForm.name || createCategoryMutation.isPending || updateCategoryMutation.isPending}
              data-testid="button-save-category"
            >
              {editingCategory ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
