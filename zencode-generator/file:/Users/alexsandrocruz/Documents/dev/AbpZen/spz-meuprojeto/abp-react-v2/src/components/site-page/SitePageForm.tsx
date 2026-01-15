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
  import { useCreateSitePage, useUpdateSitePage } from "@/lib/abp/hooks/useSitePages";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    slug: z.any(),
  
    content: z.any(),
  
    siteId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface SitePageFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function SitePageForm({
  isOpen,
  onClose,
  initialValues,
}: SitePageFormProps) {
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

  const createMutation = useCreateSitePage();
const updateMutation = useUpdateSitePage();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("SitePage updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("SitePage created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save sitepage:", error);
    toast.error(error.message || "Failed to save sitepage");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit SitePage": "Create SitePage" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the sitepage." : "Fill in the details to create a new sitepage." }
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
