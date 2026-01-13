import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetPropostalItemsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  proposalId?: string;
  }

export function usePropostalItems(input: GetPropostalItemsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["propostalItems", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/propostal-item", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllPropostalItems() {
  return useQuery({
    queryKey: ["propostalItems", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/propostal-item", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function usePropostalItem(id: string) {
  return useQuery({
    queryKey: ["propostalItem", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/propostal-item/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreatePropostalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/propostal-item", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["propostalItems"] });
    },
  });
}

export function useUpdatePropostalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/propostal-item/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["propostalItems"] });
      queryClient.invalidateQueries({ queryKey: ["propostalItem", data.id] });
    },
  });
}

export function useDeletePropostalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/propostal-item/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["propostalItems"] });
    },
  });
}


