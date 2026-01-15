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
  import { useCreateBlogPost, useUpdateBlogPost } from "@/lib/abp/hooks/useBlogPosts";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    slug: z.any(),
  
    content: z.any(),
  
    status: z.any(),
  
    siteId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface BlogPostFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function BlogPostForm({
  isOpen,
  onClose,
  initialValues,
}: BlogPostFormProps) {
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

  const createMutation = useCreateBlogPost();
const updateMutation = useUpdateBlogPost();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("BlogPost updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("BlogPost created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save blogpost:", error);
    toast.error(error.message || "Failed to save blogpost");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit BlogPost": "Create BlogPost" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the blogpost." : "Fill in the details to create a new blogpost." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="title" > Title * </Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="slug" > Slug * </Label>

<Input id="slug" {...register("slug") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="content" > Content</Label>

<Input id="content" {...register("content") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > Status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="siteId" > SiteId</Label>

<Input id="siteId" {...register("siteId") } />

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
