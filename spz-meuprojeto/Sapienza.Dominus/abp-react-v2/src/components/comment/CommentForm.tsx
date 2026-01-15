import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateComment, useUpdateComment } from "@/lib/abp/hooks/useComments";
import { toast } from "sonner";

const formSchema = z.object({
  
    entityType: z.any(),
  
    content: z.any(),
  
    entityId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface CommentFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function CommentForm({
  isOpen,
  onClose,
  initialValues,
}: CommentFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateComment();
const updateMutation = useUpdateComment();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Comment updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Comment created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save comment:", error);
    toast.error(error.message || "Failed to save comment");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Comment": "Create Comment" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the comment." : "Fill in the details to create a new comment." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="entityType" > EntityType * </Label>

<Input id="entityType" {...register("entityType") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="content" > Content * </Label>

<Input id="content" {...register("content") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="entityId" > EntityId * </Label>

<Input id="entityId" {...register("entityId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
