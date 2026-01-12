import { useState, useRef, useEffect } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { Send, MoreHorizontal, Pencil, Trash2, Reply, X, Check, MessageSquare } from "lucide-react";
import { formatDistanceToNow } from "date-fns";
import { ptBR } from "date-fns/locale";

type EntityType = 'TASK' | 'PROJECT' | 'PROPOSAL' | 'CONTRACT' | 'INVOICE' | 'LEAD';

interface CommentsProps {
  workspaceId: string;
  entityType: EntityType;
  entityId: string;
}

interface CommentData {
  id: string;
  workspaceId: string;
  entityType: string;
  entityId: string;
  authorId: string;
  content: string;
  parentId: string | null;
  isInternal: boolean;
  createdAt: string;
  updatedAt: string;
  author?: {
    id: string;
    name: string;
    avatar: string | null;
  };
}

function getInitials(name: string): string {
  return name
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase()
    .slice(0, 2);
}

export function Comments({ workspaceId, entityType, entityId }: CommentsProps) {
  const { user } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [newComment, setNewComment] = useState("");
  const [replyToId, setReplyToId] = useState<string | null>(null);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editContent, setEditContent] = useState("");
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const editInputRef = useRef<HTMLInputElement>(null);

  const queryKey = ["comments", workspaceId, entityType, entityId];

  const { data: comments = [], isLoading } = useQuery({
    queryKey,
    queryFn: () => api.getComments(workspaceId, entityType, entityId),
    enabled: !!workspaceId && !!entityId,
  });

  const createMutation = useMutation({
    mutationFn: (data: { content: string; parentId?: string }) =>
      api.createComment(workspaceId, entityType, entityId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey });
      setNewComment("");
      setReplyToId(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, content }: { id: string; content: string }) =>
      api.updateComment(workspaceId, id, content),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey });
      setEditingId(null);
      setEditContent("");
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.deleteComment(workspaceId, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey });
      setDeleteId(null);
      toast({ title: "Comentário excluído" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  useEffect(() => {
    if (editingId && editInputRef.current) {
      editInputRef.current.focus();
    }
  }, [editingId]);

  const handleSubmit = (e?: React.FormEvent) => {
    e?.preventDefault();
    if (!newComment.trim()) return;
    createMutation.mutate({
      content: newComment.trim(),
      parentId: replyToId || undefined,
    });
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSubmit();
    }
  };

  const handleEditSubmit = (id: string) => {
    if (!editContent.trim()) return;
    updateMutation.mutate({ id, content: editContent.trim() });
  };

  const startEdit = (comment: CommentData) => {
    setEditingId(comment.id);
    setEditContent(comment.content);
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditContent("");
  };

  const rootComments = (comments as CommentData[])
    .filter((c) => !c.parentId)
    .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

  const getReplies = (parentId: string) =>
    (comments as CommentData[])
      .filter((c) => c.parentId === parentId)
      .sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());

  const replyingTo = replyToId
    ? (comments as CommentData[]).find((c) => c.id === replyToId)
    : null;

  if (isLoading) {
    return (
      <div className="space-y-4" data-testid="comments-loading">
        {[...Array(3)].map((_, i) => (
          <div key={i} className="flex gap-3">
            <Skeleton className="h-8 w-8 rounded-full" />
            <div className="flex-1 space-y-2">
              <Skeleton className="h-4 w-24" />
              <Skeleton className="h-4 w-full" />
            </div>
          </div>
        ))}
      </div>
    );
  }

  return (
    <div className="space-y-4" data-testid="comments-container">
      {rootComments.length === 0 ? (
        <div className="text-center py-8 text-muted-foreground" data-testid="comments-empty">
          <MessageSquare className="mx-auto h-8 w-8 mb-2 opacity-50" />
          <p>Nenhum comentário ainda</p>
        </div>
      ) : (
        <div className="space-y-4" data-testid="comments-list">
          {rootComments.map((comment) => (
            <CommentItem
              key={comment.id}
              comment={comment}
              currentUserId={user?.id}
              isEditing={editingId === comment.id}
              editContent={editContent}
              editInputRef={editInputRef}
              onEditContentChange={setEditContent}
              onEditSubmit={() => handleEditSubmit(comment.id)}
              onEditCancel={cancelEdit}
              onStartEdit={() => startEdit(comment)}
              onDelete={() => setDeleteId(comment.id)}
              onReply={() => {
                setReplyToId(comment.id);
                inputRef.current?.focus();
              }}
              isUpdating={updateMutation.isPending && editingId === comment.id}
            >
              {getReplies(comment.id).map((reply) => (
                <CommentItem
                  key={reply.id}
                  comment={reply}
                  currentUserId={user?.id}
                  isEditing={editingId === reply.id}
                  editContent={editContent}
                  editInputRef={editInputRef}
                  onEditContentChange={setEditContent}
                  onEditSubmit={() => handleEditSubmit(reply.id)}
                  onEditCancel={cancelEdit}
                  onStartEdit={() => startEdit(reply)}
                  onDelete={() => setDeleteId(reply.id)}
                  isReply
                  isUpdating={updateMutation.isPending && editingId === reply.id}
                />
              ))}
            </CommentItem>
          ))}
        </div>
      )}

      <form onSubmit={handleSubmit} className="flex gap-2 items-start pt-4 border-t" data-testid="comment-form">
        <Avatar className="h-8 w-8">
          <AvatarImage src={user?.avatar || undefined} />
          <AvatarFallback>{user?.name ? getInitials(user.name) : "?"}</AvatarFallback>
        </Avatar>
        <div className="flex-1 space-y-2">
          {replyingTo && (
            <div className="flex items-center gap-2 text-sm text-muted-foreground bg-muted px-2 py-1 rounded">
              <Reply className="h-3 w-3" />
              <span>Respondendo a {replyingTo.author?.name || "Usuário"}</span>
              <Button
                type="button"
                variant="ghost"
                size="icon"
                className="h-5 w-5 ml-auto"
                onClick={() => setReplyToId(null)}
                data-testid="button-cancel-reply"
              >
                <X className="h-3 w-3" />
              </Button>
            </div>
          )}
          <div className="flex gap-2">
            <Input
              ref={inputRef}
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder="Escreva um comentário..."
              disabled={createMutation.isPending}
              data-testid="input-new-comment"
            />
            <Button
              type="submit"
              size="icon"
              disabled={!newComment.trim() || createMutation.isPending}
              data-testid="button-send-comment"
            >
              <Send className="h-4 w-4" />
            </Button>
          </div>
        </div>
      </form>

      <AlertDialog open={!!deleteId} onOpenChange={(open) => !open && setDeleteId(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Excluir Comentário</AlertDialogTitle>
            <AlertDialogDescription>
              Tem certeza que deseja excluir este comentário? Esta ação não pode ser desfeita.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel data-testid="button-cancel-delete">Cancelar</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => deleteId && deleteMutation.mutate(deleteId)}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
              data-testid="button-confirm-delete"
            >
              Excluir
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}

interface CommentItemProps {
  comment: CommentData;
  currentUserId?: string;
  isEditing: boolean;
  editContent: string;
  editInputRef: React.RefObject<HTMLInputElement | null>;
  onEditContentChange: (value: string) => void;
  onEditSubmit: () => void;
  onEditCancel: () => void;
  onStartEdit: () => void;
  onDelete: () => void;
  onReply?: () => void;
  isReply?: boolean;
  isUpdating?: boolean;
  children?: React.ReactNode;
}

function CommentItem({
  comment,
  currentUserId,
  isEditing,
  editContent,
  editInputRef,
  onEditContentChange,
  onEditSubmit,
  onEditCancel,
  onStartEdit,
  onDelete,
  onReply,
  isReply,
  isUpdating,
  children,
}: CommentItemProps) {
  const isOwn = currentUserId === comment.authorId;
  const authorName = comment.author?.name || "Usuário";
  const authorAvatar = comment.author?.avatar;

  return (
    <div className={isReply ? "ml-10 mt-3" : ""} data-testid={`comment-item-${comment.id}`}>
      <div className="flex gap-3 group">
        <Avatar className="h-8 w-8">
          <AvatarImage src={authorAvatar || undefined} />
          <AvatarFallback>{getInitials(authorName)}</AvatarFallback>
        </Avatar>
        <div className="flex-1 min-w-0">
          <div className="flex items-center gap-2">
            <span className="font-medium text-sm">{authorName}</span>
            <span className="text-xs text-muted-foreground">
              {formatDistanceToNow(new Date(comment.createdAt), {
                addSuffix: true,
                locale: ptBR,
              })}
            </span>
            {isOwn && !isEditing && (
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button
                    variant="ghost"
                    size="icon"
                    className="h-6 w-6 opacity-0 group-hover:opacity-100 transition-opacity"
                    data-testid={`button-comment-menu-${comment.id}`}
                  >
                    <MoreHorizontal className="h-4 w-4" />
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuItem onClick={onStartEdit} data-testid={`button-edit-comment-${comment.id}`}>
                    <Pencil className="h-4 w-4 mr-2" />
                    Editar
                  </DropdownMenuItem>
                  <DropdownMenuItem
                    onClick={onDelete}
                    className="text-destructive"
                    data-testid={`button-delete-comment-${comment.id}`}
                  >
                    <Trash2 className="h-4 w-4 mr-2" />
                    Excluir
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            )}
          </div>
          {isEditing ? (
            <div className="flex gap-2 mt-1">
              <Input
                ref={editInputRef}
                value={editContent}
                onChange={(e) => onEditContentChange(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === "Enter" && !e.shiftKey) {
                    e.preventDefault();
                    onEditSubmit();
                  } else if (e.key === "Escape") {
                    onEditCancel();
                  }
                }}
                disabled={isUpdating}
                data-testid={`input-edit-comment-${comment.id}`}
              />
              <Button
                size="icon"
                variant="ghost"
                onClick={onEditSubmit}
                disabled={!editContent.trim() || isUpdating}
                data-testid={`button-save-edit-${comment.id}`}
              >
                <Check className="h-4 w-4" />
              </Button>
              <Button
                size="icon"
                variant="ghost"
                onClick={onEditCancel}
                disabled={isUpdating}
                data-testid={`button-cancel-edit-${comment.id}`}
              >
                <X className="h-4 w-4" />
              </Button>
            </div>
          ) : (
            <p className="text-sm text-foreground mt-0.5 whitespace-pre-wrap break-words">
              {comment.content}
            </p>
          )}
          {!isReply && onReply && !isEditing && (
            <Button
              variant="ghost"
              size="sm"
              className="h-6 px-2 text-xs text-muted-foreground hover:text-foreground mt-1"
              onClick={onReply}
              data-testid={`button-reply-${comment.id}`}
            >
              <Reply className="h-3 w-3 mr-1" />
              Responder
            </Button>
          )}
        </div>
      </div>
      {children}
    </div>
  );
}
