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
  import { useCreateBlogPostVersion, useUpdateBlogPostVersion } from "@/lib/abp/hooks/useBlogPostVersions";
import { toast } from "sonner";

const formSchema = z.object({
  
    versionNumber: z.any(),
  
    postId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface BlogPostVersionFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function BlogPostVersionForm({
  isOpen,
  onClose,
  initialValues,
}: BlogPostVersionFormProps) {
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

  const createMutation = useCreateBlogPostVersion();
const updateMutation = useUpdateBlogPostVersion();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("BlogPostVersion updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("BlogPostVersion created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save blogpostversion:", error);
    toast.error(error.message || "Failed to save blogpostversion");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit BlogPostVersion": "Create BlogPostVersion" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the blogpostversion." : "Fill in the details to create a new blogpostversion." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="versionNumber" > VersionNumber * </Label>

<Input id="versionNumber" type = "number" step = "any" {...register("versionNumber") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="postId" > PostId</Label>

<Input id="postId" {...register("postId") } />

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
